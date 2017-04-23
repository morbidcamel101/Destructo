using UnityEngine;
using System.Collections;

public abstract class MovementMotorBase : BehaviorBase
{
	public enum State { Idle, Move, Moving, Stop, Stopping, Stopped };

	public ITarget Target;
	public State state = State.Idle;
	public float speed;


	void Awake()
	{
		enabled = false;
		state = State.Idle;
	}

	void OnEnable()
	{
		
	}

	// TODO
	public virtual void MoveTo(ITarget target)
	{
		Target = target;
		state = State.Move;
	}


}

