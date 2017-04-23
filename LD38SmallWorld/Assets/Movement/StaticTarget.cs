﻿using System;
using UnityEngine;

[Serializable]
public sealed class StaticTarget: TargetBase
{
	public StaticTarget(Vector3 position, Vector3 direction)
	{
		this.position = position;
		this.direction = direction;
	}

	private Vector3 position;
	public override Vector3 Position
	{
		get { return position; }
	}

	public Vector3 direction;
	public override Vector3 Direction
	{
		get { return direction; }
	}
}
