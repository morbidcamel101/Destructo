using UnityEngine;
using System.Collections;
using System;

[AddComponentMenu("Small World/Thug")]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(MovementMotorBase))]
public class Thug : CharacterBase
{
	public enum State { Seeking, Attack, Attacking, Attacked, Hide, Hiding }
	public float strengthMultiplier = 1f;
	public float alertness = 0.5f;
	public State state;
	public SphereCollider detection;
	public float detectionRadius { get { return detection.radius; } }
	private MovementMotorBase movement;
	private CharacterBase currentTarget;
	private float resumeTime;
	private Collider bodyCollider;

	void Awake()
	{
		// use the strength multiplier
		Health.totalHealth *= strengthMultiplier;
		Health.currentHealth *= strengthMultiplier;
		Health.regenerationRate *= strengthMultiplier; 
		bodyCollider = this.GetComponent<Collider>();
		Ensure(bodyCollider.material); // Make sure we can get the impact effect!
		Ensure(detection);
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

			resumeTime = Time.time + alertness;

			if (currentTarget != null)
			{
				
				break;
			}
			// TODO - Raycast towards the player if we have a currentTarget - see if he is in line of site
			Ray ray = new Ray(transform.position, (currentTarget.transform.position - transform.position).normalized);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, detectionRadius))
			{
				if (currentTarget = hit.collider.GetComponentInParent<Player>())
				{
					movement.MoveTo(new DynamicTarget(this.transform, currentTarget.transform));
					state = State.Attack;
				}
			}
			break;

			case State.Attack:
				this.Log("Target locked");
				// TODO
				break;
		}
	}


	protected override void OnDeath ()
	{
		throw new NotImplementedException(); // TODO
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

	protected override void OnImpact (Bullet bullet)
	{
		currentTarget = bullet.GetComponentInParent<CharacterBase>();
		this.Ensure(currentTarget, "Bullet not shot from a character?");
	}
}

