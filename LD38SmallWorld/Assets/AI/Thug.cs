using UnityEngine;
using System.Collections;
using System;
using UnityEngine.AI;

[AddComponentMenu("Small World/Thug")]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(MovementMotorBase))]
[RequireComponent(typeof(Impact))]
public class Thug : CharacterBase
{
	public enum State { Seeking, Dodge, Aim, Attack, Attacking, Attacked, Hide, Hiding }
	public float strengthMultiplier = 1f;
	public float alertness = 0.5f;
	public State state;
	public float decisionDelay = 1f;
	public float minInspectionDelay = 5f;
	public float maxInspectionDelay = 15f;
	public float detectionRadius = 100f;
	public Prototype smokePrefab;
	public float targetOffset = 10f; // Basically the most dangerous distance ;)
	public float raycastInterval = 0.2f;

	private MovementMotorBase movement;
	private Motor motor;
	private float resumeTime;
	internal int points = 100;
	private Collider bodyCollider;
	private float alertnessFactor = 0f;

	void Awake()
	{
		// use the strength multiplier
		alertnessFactor = 1f/alertness;
		Health.totalHealth *= strengthMultiplier;
		Health.currentHealth *= strengthMultiplier;
		//Health.regenerationDuration *= strengthMultiplier; 
		bodyCollider = this.GetComponent<Collider>();
		if (movement == null)
			movement = GetComponent<MovementMotorBase>();
		Ensure(bodyCollider.material); // Make sure we can get the impact effect!
		// Unity Bug with trigger colliders and PhysX check my clips on twitch.tv/morbidcamel101 -
		// > http://answers.unity3d.com/questions/962142/what-is-physx-postislandgen-and-how-can-i-reduce-i.html?childToView=962652#answer-962652
		//Ensure(detection);
		Ensure(movement);
		//this.Assert(detection.isTrigger, "The detection sphere must be a trigger!");
		resumeTime = Time.time + UnityEngine.Random.Range(minInspectionDelay, maxInspectionDelay) * alertness;
		state = State.Seeking;
		motor = new Motor();

	}

	void Update()
	{
		
		switch(state)
		{
			case State.Seeking:
				if (Time.time < resumeTime)
					return;

				if (motor.attackTarget != null)
				{
					state = State.Attack;
					break;
				}
				state = State.Aim;
				resumeTime = Time.time + decisionDelay * alertnessFactor;
				break;

			case State.Aim:
				if (LockOn())
				{
					state = State.Attack;
					break;
				}
				// Exit stage left - Snagglepuss
				resumeTime = Time.time + UnityEngine.Random.Range(minInspectionDelay, maxInspectionDelay) * alertnessFactor;
				state = State.Dodge;
				break;

			case State.Dodge:
				Log("{0} Dodge", this.gameObject);

				var target = CharacterManager.Instance.GetRandomPosition(transform.position, Mathf.Clamp(UnityEngine.Random.value * detectionRadius, 0.1f, detectionRadius));
				if (target == null)
				{
					state = State.Seeking;
					break;
				}
				motor.movementTarget = new StaticTarget(target.Value, (target.Value - transform.position).normalized);
				movement.MoveTo(motor.movementTarget);
				state = State.Seeking;
				break;

			case State.Attack:
				this.Log("Target locked");
				// TODO
				state = State.Attacking;
				movement.MoveTo(motor.movementTarget);
				resumeTime = Time.time + (decisionDelay * alertness); // Longer wait time the more relentless!
				break;

			case State.Attacking:
				if (motor.attackTarget == null)
				{
					state = State.Attacked;
					break;
				}

				FireAt(motor.attackTarget);

				if (Time.time < resumeTime)
					break;

				// Raycast to see if the target is in line of sight
				resumeTime = Time.time + (decisionDelay * alertness); // Longer wait time the more relentless 
				if (!LockOn())
				{
					// Can't see target
					state = State.Attacked;
				}
				break;

			case State.Attacked:
				// STOP Firing!!
				StopFire();
				motor.attackTarget = null;
				state = State.Seeking;
				break;
		}
	}

	private bool LockOn()
	{
		var player = Player;

		Ray ray = new Ray(transform.position, (player.transform.position - transform.position).normalized);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, detectionRadius))
		{
			var dyn = motor.attackTarget as DynamicTarget;
			if (dyn != null && hit.transform == dyn.target)
				return true;

			var p = hit.collider.GetComponentInParent<Player>();
			if (p != null)
			{
				motor.attackTarget = GetTarget(hit.transform);
				motor.movementTarget = GetMovementTarget(hit.transform, hit.normal * targetOffset);
				//movement.MoveTo(GetMovementTarget(hit.transform, hit.normal * targetOffset));
				return true;
			}
		}
		return false;
	}

	private DynamicTarget GetTarget(Transform target)
	{
		return new DynamicTarget(transform, target);
	}

	private DynamicTarget GetMovementTarget(Transform target, Vector3 offset)
	{
		return new DynamicTarget(this.transform, target, offset);
	}




	protected override void OnDeath ()
	{
		
		CharacterManager.Instance.Dead(this.gameObject);
		Player.score += points;
	}

	protected override void OnCriticalHealth ()
	{
		// TODO - Tint with blood!
		Log("Crititcal health reached! -- DUCK");
	}

	protected override void OnLowHealth ()
	{
		state = State.Dodge;
	}

	protected override void CanImpact (Bullet bullet)
	{
		if (bullet.sender is Player)
			bullet.Hit();
		else 
			return;

		if (Health.IsLow)
		{
			if (smokePrefab.prefab)
			{
				var obj = Spawner.Spawn(smokePrefab, true, bullet.transform.position, Quaternion.identity);
				Spawner.Recycle(obj, 5f);
			}
		}
	}

	protected override void OnImpact (Bullet bullet)
	{
		var dyn = motor.attackTarget as DynamicTarget;
		if (dyn == null || dyn.target != bullet.sender.transform)
		{
			motor.attackTarget = GetTarget(bullet.sender.transform); // Return to sender
		}


		Player.score += Convert.ToInt32(bullet.damage);

		if (Time.time > resumeTime)
		{
			resumeTime = Time.time + this.alertnessFactor;
			state = State.Dodge; // Move out the way
		}
		// Whysics
		GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bullet.force, ForceMode.Impulse);

	}

	public override void Spawn (float strengthMultiplier)
	{
		this.strengthMultiplier = strengthMultiplier;
		this.Health.totalHealth = 100 * strengthMultiplier;
		this.Health.Reset();

		alertness *= strengthMultiplier;
		detectionRadius *= strengthMultiplier;
		movement.speed *= strengthMultiplier;
		points = (int)(points * strengthMultiplier);

		// Not that simple
		//transform.localScale = Vector3.ClampMagnitude(transform.localScale * strengthMultiplier, 5f);

		for(int i = 0; i < holdsters.Length; i++)
		{
			holdsters[i].gun.Reset(this.strengthMultiplier);
		}
		base.Spawn (strengthMultiplier);
	}
}

