using System;
using UnityEngine;




public interface ITarget
{
	Vector3 Position { get; }

	Vector3 Direction { get; }

	float GetDistance(Vector3 source);

	float GetDistanceSqr(Vector3 source);

	Vector3 GetDirection(Vector3 source);

	bool InRange(Vector3 source, float range);

	bool IsReady {get; }


}

