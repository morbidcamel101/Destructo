﻿using System;
using UnityEngine;




public interface ITarget
{
	Vector3 Position { get; }

	Vector3 Direction { get; }

	float GetDistance(Vector3 source);

	Vector3 GetDirection(Vector3 source);

}

