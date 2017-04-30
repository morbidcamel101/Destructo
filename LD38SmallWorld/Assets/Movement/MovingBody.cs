using UnityEngine;
using System.Collections;

public abstract class MovingBody : BehaviorBase
{

	public Vector3 currentVelocity;
	public Vector3 currentLocalVelocity;
	public Vector3 currentAcceleration;
	public float currentSpeed;
	public Vector3 lastVelocity;
	public Vector3 lastPosition;

	protected virtual void PhysicsUpdate()
	{
		currentVelocity = CalculateVelocity(transform.position, lastPosition, Time.fixedDeltaTime);
		currentLocalVelocity = transform.InverseTransformDirection(currentVelocity);
		currentLocalVelocity.y = 0;
		currentSpeed = currentLocalVelocity.magnitude;
		currentAcceleration = (currentVelocity - lastVelocity) / Time.fixedDeltaTime;
		lastPosition = transform.position;
		lastVelocity = currentVelocity;
	}

	protected Vector3 CalculateVelocity(Vector3 position, Vector3 lastPosition, float t)
	{
		if (t == 0f)
			return Vector3.zero;

		return (position - lastPosition) / t;
	}
}

