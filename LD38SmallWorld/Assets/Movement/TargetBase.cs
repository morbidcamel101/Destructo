using System;
using UnityEngine;

public abstract class TargetBase: ITarget
{

	public abstract Vector3 Position { get; }
	public abstract Vector3 Direction {get; }

	public abstract bool IsReady {get;}

	public float GetDistanceSqr(Vector3 source)
	{
		return (Position - source).sqrMagnitude;
	}

	public float GetDistance(Vector3 source)
	{
		return (Position - source).magnitude;
	}

	public Vector3 GetDirection(Vector3 source)
	{
		return (Position - source).normalized;
	}

	public virtual bool InRange(Vector3 source, float range)
	{
		return (Position - source).sqrMagnitude < (range * range);
	}

	public virtual bool IsSame(ITarget other)
	{
		return other != null && other.GetType() == this.GetType() && other.Position == this.Position;
	}

	public abstract void CopyFrom(ITarget other);

}

