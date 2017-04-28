using UnityEngine;
using System.Collections;

public class NavTarget : TargetBase
{
	public NavTarget(NavMeshMovement movement)
	{
		this.movement = movement;
	}

	NavMeshMovement movement;

	public override Vector3 Position {
		get {
			return movement.agent.nextPosition;
		}
	}

	public override Vector3 Direction {
		get {
			return (Position - movement.transform.position).normalized;
		}
	}

	public override bool IsReady { get { return movement.agent.hasPath; } }


	public override bool InRange(Vector3 source, float range)
	{
		return (movement.agent.pathEndPosition - movement.transform.position).sqrMagnitude < range*range;
	}

}

