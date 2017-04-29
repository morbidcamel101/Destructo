using UnityEngine;
using System.Collections;
using System;


[AddComponentMenu("Small World/Bullet")]
public class Bullet : BehaviorBase
{
	public float speed = 10;
	public float lifeTime = 0.5f;
	public float distance = 10000;
	public float damage = 10f;
	public float force = 1f;
	public float strengthMultiplier = 1f;
	internal bool didHit = false;

	public CharacterBase sender;

	private Transform trans;
	internal ITarget target;
	private float spawnTime;
	private Rigidbody rigid;
	private float currentDistance;
	private float currentSpeed;


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
	void Update ()
	{
		
		var delta = target.GetDirection(trans.position) * currentSpeed * Time.deltaTime;
		trans.position += delta;
		currentSpeed = speed * strengthMultiplier;
		currentDistance -= currentSpeed * Time.deltaTime;


		if (Time.time > spawnTime + lifeTime)
		{
			Recycle();
		}

		if (currentDistance < 0f)
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
		spawnTime = Time.time;
		Spawner.Recycle(gameObject, lifeTime);
	}

	public virtual void Hit() 
	{
		didHit = true;

	}

	public void Recycle()
	{
		enabled = false;
		Spawner.Recycle(gameObject, 0.01f);
	}
}

