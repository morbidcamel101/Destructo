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
	public float force = 100f;
	public float strengthMultiplier = 1f;
	internal bool didHit = false;

	public CharacterBase sender;

	private Transform trans;
	private float spawnTime;
	private Rigidbody rigid;


	void Awake()
	{
		enabled = false;
		rigid = GetComponent<Rigidbody>();
		this.Assert(damage > 0, "Bullet cannot have zero damage");
	}


	// Update is called once per frame
	void Update ()
	{
		
		var delta = trans.forward * speed * Time.deltaTime;
		trans.position += delta;
		distance -= speed * Time.deltaTime;

		if (Time.time > spawnTime + lifeTime)
		{
			Recycle();
		}
	}

	public virtual void Shoot(CharacterBase sender)
	{
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
		Recycle();
	}

	public void Recycle()
	{
		enabled = false;
		if (rigid)
			rigid.isKinematic = true;
		Spawner.Recycle(gameObject);
	}
}

