using System;
using UnityEngine;

[Serializable]
public sealed class DynamicTarget: TargetBase
{
	public DynamicTarget(Transform source, Transform target, Vector3 offset = default(Vector3))
	{
		this.source = source;
		this.target = target;
		this.offset = offset;

	}

	public Transform source;
	public Transform target;
	public Vector3 offset;


	public override Vector3 Position {
		get {
			return target != null ? (target.position + offset) : Vector3.zero;
		}
	}

	public override Vector3 Direction {
		get {
			return ((target.position + offset) - source.position).normalized;
			
		}
	}

	public override bool IsReady { get { return Position != Vector3.zero; } }

	public override bool IsSame (ITarget other)
	{
		DynamicTarget dyn = other as DynamicTarget;
		if (dyn != null)
		{
			return dyn.target == target;
		}
		return base.IsSame (other);
	}

	public override void CopyFrom (ITarget other)
	{
		var dyn = other as DynamicTarget;
		if (dyn == null)
			return;

		this.source = dyn.source;
		this.target = dyn.target;
		this.offset = dyn.offset;
	}
}

