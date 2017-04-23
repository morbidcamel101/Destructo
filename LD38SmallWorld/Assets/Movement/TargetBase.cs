using System;
using UnityEngine;

public abstract class TargetBase: ITarget
{

	public abstract Vector3 Position { get; }
	public abstract Vector3 Direction {get; }

	public float GetDistanceSqr(Vector3 target)
	{
		return (target - Position).sqrMagnitude;
	}

	public float GetDistance(Vector3 target)
	{
		return (target - Position).magnitude;
	}

	public Vector3 DirectionTo(Vector3 source)
	{
		return (source - Position).normalized;
	}
}

