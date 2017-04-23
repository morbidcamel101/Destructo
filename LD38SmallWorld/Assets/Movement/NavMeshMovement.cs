using System;
using UnityEngine;
using UnityEngine.AI;

[AddComponentMenu("Small World/Nav Mesh Movement")]
[RequireComponent(typeof(NavMeshAgent))]
public sealed class NavMeshMovement: MovementMotorBase
{
	private NavMeshAgent agent;

	void Awake()
	{
		if (agent == null)
			agent = GetComponent<NavMeshAgent>();

		Ensure(agent);
	}

	void FixedUpdate()
	{
		if (Target == null)
			return;

		this.transform.position =  Vector3.Slerp(this.transform.position, this.transform.position + (Target.GetDirection(transform.position) * speed), Time.deltaTime);
		// TODO - Rotation
	}

	public override void MoveTo (ITarget target)
	{
		base.MoveTo (target);
		this.agent.SetDestination(target.Position);
	}
}

