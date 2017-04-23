using System;
using UnityEngine;

[Serializable]
public sealed class DynamicTarget: TargetBase
{
	public DynamicTarget(Transform source, Transform target)
	{
		this.source = source;
		this.target = target;

	}

	public Transform source;
	public Transform target;


	public override Vector3 Position {
		get {
			return target != null ? target.position : Vector3.zero;
		}
	}

	public override Vector3 Direction {
		get {
			return (target.position - source.position).normalized;
			
		}
	}
}

