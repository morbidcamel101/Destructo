using System;
using UnityEngine;
using UnityEngine.AI;

[AddComponentMenu("Small World/Nav Mesh Movement")]
[RequireComponent(typeof(NavMeshAgent))]
public sealed class NavMeshMovement: MovementMotorBase
{
	private NavMeshAgent agent;
	public bool agentActive;


	void Awake()
	{
		if (agent == null)
			agent = GetComponent<NavMeshAgent>();
		
		Ensure(agent);
	}

	void FixedUpdate()
	{
		agent.speed = this.speed;

		if (Target == null)
		{
			enabled = false;
			return;
		}

		Debug.DrawLine(this.transform.position, agent.nextPosition, Color.yellow);
		if (agentActive)
		{
			this.transform.position =  Vector3.Slerp(this.transform.position, agent.nextPosition, Time.deltaTime*speed);	
		}
		else
		{
			// LD38 Code
			this.transform.position =  Vector3.Slerp(this.transform.position, this.transform.position + (Target.GetDirection(transform.position) * speed), Time.deltaTime);

		}
		// TODO - Rotation
	}

	public override void MoveTo (ITarget target)
	{
		
		base.MoveTo (target);
		agentActive = this.agent.isOnNavMesh;
		if (agentActive)
		{
			this.agent.SetDestination(target.Position);
		}
	}
}

