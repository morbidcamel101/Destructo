using UnityEngine;
using System.Collections;
using System;


[AddComponentMenu("Small World/Bullet")]
public class Bullet : MovingBody
{
	public float speed = 10;
	public float lifeTime = 0.5f;
	public float distance = 10000;
	public float damage = 10f;
	public float force = 1f;
	public float strengthMultiplier = 1f;
	public bool didHit = false;
	public float currentDistance;
	public float hitDistance = 0.01f;
	public CharacterBase sender;
	private Transform trans;
	internal ITarget target;
	private Rigidbody rigid;



	void Awake()
	{
		enabled = false;
		this.Assert(damage > 0, "Bullet cannot have zero damage");
	}

	void OnEnable()
	{
		currentDistance = distance;
	}


	// Update is called once per frame
	void FixedUpdate ()
	{
		PhysicsUpdate();

		var sp = speed * strengthMultiplier;
		var delta = target.Direction * sp * Time.fixedDeltaTime;
		trans.position += delta;
		currentDistance -= sp * Time.deltaTime;
		if (currentDistance < 0f || didHit)
		{
			Recycle();
		}
		var distSqr = hitDistance * hitDistance;
		var dist = target.GetDistanceSqr(trans.position);
		if (dist <= distSqr)
		{
			Recycle();
		}
	}

	public virtual void Shoot(CharacterBase sender, ITarget target)
	{
		this.target = target;
		this.sender = sender;
		didHit = false;
		enabled = true;
		trans = transform;
		Spawner.Recycle(gameObject, lifeTime);
	}

	public virtual void Hit() 
	{
		didHit = true;
		Recycle();
	}

	public void Recycle()
	{
		enabled = false;
		Spawner.Recycle(gameObject);
	}
}

