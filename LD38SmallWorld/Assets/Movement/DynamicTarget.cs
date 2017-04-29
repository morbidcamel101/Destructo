using System;
using UnityEngine;

[Serializable]
public sealed class DynamicTarget: TargetBase
{
	public DynamicTarget(Transform source, Transform target, float offset = 0)
	{
		this.source = source;
		this.target = target;
		this.offset = offset;

	}

	public Transform source;
	public Transform target;
	public float offset;


	public override Vector3 Position {
		get {
			return target != null ? (target.position + Direction * offset) : Vector3.zero;
		}
	}

	public override Vector3 Direction {
		get {
			return (target.position - source.position).normalized;
			
		}
	}

	public override bool IsReady { get { return Position != Vector3.zero; } }
}

