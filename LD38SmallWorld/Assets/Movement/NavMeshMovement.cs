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
		this.transform.position =  Vector3.Slerp(transform.position, agent.nextPosition, Time.deltaTime);
		// TODO - Rotation
	}

	public override void MoveTo (ITarget target)
	{
		base.MoveTo (target);
		this.agent.SetDestination(target.Position);
	}
}

