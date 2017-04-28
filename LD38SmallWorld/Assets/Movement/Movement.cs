using UnityEngine;
using System.Collections;

public abstract class MovementMotorBase : BehaviorBase
{
	public enum State { Idle, Move, Moving, Stop, Stopping, Stopped };

	public ITarget Target;
	public State state = State.Idle;
	public float speed = 5;


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
		enabled = true;
		Target = target;
		state = State.Move;
	}

	public virtual void ResolvePath()
	{
	}


}

