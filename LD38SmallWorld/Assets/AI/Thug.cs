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
	public enum State { Seeking, Inspect, Attack, Attacking, Attacked, Hide, Hiding }
	public float strengthMultiplier = 1f;
	public float alertness = 0.5f;
	public State state;
	public float decisionDelay = 10f;
	public float minInspectionDelay = 5f;
	public float maxInspectionDelay = 15f;
	public SphereCollider detection;
	public float detectionRadius { get { return detection.radius; } }
	private MovementMotorBase movement;
	private CharacterBase currentTarget;
	private float resumeTime;
	private float inspectTime;
	public Prototype smokePrefab;
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
		Ensure(detection);
		Ensure(movement);
		this.Assert(detection.isTrigger, "The detection sphere must be a trigger!");
		inspectTime = Time.time + UnityEngine.Random.Range(minInspectionDelay, maxInspectionDelay) * alertness;
		state = State.Seeking;

	}

	void Update()
	{
		
		switch(state)
		{
			case State.Seeking:
			if (Time.time < resumeTime)
				return;

			resumeTime = Time.time + decisionDelay * alertnessFactor;

			if (currentTarget != null)
			{
				state = State.Attack;
				break;
			}

			if (LockOn())
			{
				state = State.Attack;
			}

			if (Time.time > inspectTime)
			{
				state = State.Inspect;
			}
			break;

			case State.Inspect:
			Log("{0} Inspecting", this.gameObject);
			var target = CharacterManager.Instance.GetRandomPosition(transform.position, UnityEngine.Random.value * detectionRadius);
			if (target == null)
			{
				state = State.Seeking;
				break;
			}
			movement.MoveTo(new StaticTarget(target.Value, (target.Value - transform.position).normalized));
			inspectTime = Time.time + UnityEngine.Random.Range(minInspectionDelay, maxInspectionDelay) * alertnessFactor;
			state = State.Seeking;
			break;

			case State.Attack:
				this.Log("Target locked");
				// TODO
				state = State.Attacking;
				break;

			case State.Attacking:
				if (!currentTarget)
				{
					state = State.Attacked;
					break;
				}
				FireAt(currentTarget.transform);
				
				if (!LockOn())
				{
					state = State.Attacked;
				}
				break;

			case State.Attacked:
				// TODO - Reload?
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
			if (currentTarget = hit.collider.GetComponentInParent<Player>())
			{
				movement.MoveTo(new DynamicTarget(this.transform, currentTarget.transform));
				return true;
			}
		}
		return false;
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
				var obj = Spawner.Spawn(smokePrefab, false, bullet.transform.position, Quaternion.identity);
				Spawner.Recycle(obj, 5f);
			}
		}
	}

	protected override void OnImpact (Bullet bullet)
	{
		currentTarget = bullet.sender;
		Player.score += Convert.ToInt32(bullet.damage);
		state = State.Inspect; // Move out the way

		//GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bullet.force, ForceMode.Impulse);
	}

	public override void Randomize ()
	{
		this.Health.totalHealth = 100 * strengthMultiplier;
		this.Health.Reset();

		alertness *= strengthMultiplier;
		detection.radius *= strengthMultiplier;
		movement.speed *= strengthMultiplier;

		for(int i = 0; i < holdsters.Length; i++)
		{
			holdsters[i].gun.strengthMultiplier = this.strengthMultiplier;
			holdsters[i].gun.Reset();

		}
		base.Randomize ();
	}
}

