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
	public SphereCollider detection;
	public float detectionRadius { get { return detection.radius; } }
	private MovementMotorBase movement;
	private CharacterBase currentTarget;
	private float resumeTime;
	public float inspectTime;
	private Collider bodyCollider;

	void Awake()
	{
		// use the strength multiplier
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
		state = State.Seeking;
	}

	void Update()
	{
		switch(state)
		{
			case State.Seeking:
			if (Time.time < resumeTime)
				return;

			UpdateNav(); 
			resumeTime = Time.time + alertness;

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
			var target = CharacterManager.Instance.GetRandomPosition(transform.position, UnityEngine.Random.value * 50f);
			if (target == null)
			{
				state = State.Seeking;
				break;
			}
			movement.MoveTo(new StaticTarget(target.Value, (target.Value - transform.position).normalized));
			inspectTime = Time.time + inspectTime;
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
		// TODO - Raycast towards the player if we have a currentTarget - see if he is in line of site


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
	}

	protected override void OnCriticalHealth ()
	{
		// TODO - Tint with blood!
		Log("Crititcal health reached! -- DUCK");
	}

	protected override void OnLowHealth ()
	{
		// TODO - Health is low --- find cover

	}

	protected override void CanImpact (Bullet bullet)
	{
		if (bullet.sender is Player)
			bullet.Hit();
	}

	protected override void OnImpact (Bullet bullet)
	{
		currentTarget = bullet.sender;
		Player.score += Convert.ToInt32(bullet.damage);

		GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bullet.force, ForceMode.Impulse);
	}

	public override void Randomize ()
	{
		this.strengthMultiplier = UnityEngine.Random.Range(1f, CharacterManager.Instance.strengthMultiplier);
		this.Health.Reset();
		base.Randomize ();
	}

	internal void UpdateNav()
	{
		var agent = GetComponent<NavMeshAgent>();
		if (agent != null && !agent.enabled)
		{
			agent.enabled = true;
		}
	}
}

