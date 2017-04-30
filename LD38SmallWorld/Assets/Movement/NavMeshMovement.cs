using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[AddComponentMenu("Small World/Nav Mesh Movement")]
[RequireComponent(typeof(NavMeshAgent))]
public sealed class NavMeshMovement: MovementMotorBase
{
	internal NavMeshAgent agent;
	private float distanceSqr;
	public bool agentActive;
	public float targetRange = 0.5f;
	public float navAgentMinDistance = 10f;

	void Awake()
	{
		if (agent == null)
			agent = GetComponent<NavMeshAgent>();

		Ensure(agent);
	}



	void FixedUpdate()
	{
		PhysicsUpdate();
		if (Target == null || !Target.IsReady)
		{
			enabled = false;
			return;
		}

		// LD38 Code
		this.transform.position =  Vector3.Lerp(this.transform.position, this.transform.position + (Target.GetDirection(transform.position) * speed), Time.fixedDeltaTime);

		Debug.DrawLine(transform.position, Target.Position);

		// TODO - Rotation
		if (Target.InRange(transform.position, targetRange))
		{
			enabled = false;
		}

	}

	public override void MoveTo (ITarget target)
	{
		
		base.MoveTo (target);

		if (target.GetDistanceSqr(transform.position) < navAgentMinDistance * navAgentMinDistance)
		{
			agentActive = false;
			return;
		}

		if (agentActive = this.agent.isOnNavMesh)
		{
			agent.ResetPath();
			CharacterManager.Instance.QueueResolve(this);
		}
	}

	public override void ResolvePath ()
	{
		base.ResolvePath ();

		agentActive = this.agent.isOnNavMesh;
		agent.speed = this.speed;
		if (agentActive)
		{
			this.agent.SetDestination(Target.Position);
			this.agent.updatePosition = false;
			this.agent.isStopped = false;

			this.Target = new NavTarget(this); // Locked on - cansas city shuffle
		}
	}
}

