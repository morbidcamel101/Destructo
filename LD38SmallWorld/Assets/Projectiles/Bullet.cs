﻿using UnityEngine;
using System.Collections;
using System;


[AddComponentMenu("Small World/Bullet")]
public class Bullet : BehaviorBase
{
	public enum Mode {
		RigidBody,
		Simple
	}

	public Mode mode = Mode.RigidBody;
	public float speed = 10;
	public float lifeTime = 0.5f;
	public float distance = 10000;
	public float damage = 10f;

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


	void OnDisable() {
		rigid.isKinematic = true;
	}

	// Update is called once per frame
	void Update ()
	{
		if (mode == Mode.Simple)
		{
			trans.position += trans.forward * speed * Time.deltaTime;
			distance -= speed * Time.deltaTime;
		}

		if (Time.time > spawnTime + lifeTime)
		{
			Recycle();
		}
	}

	public virtual void Shoot(CharacterBase sender)
	{
		this.sender = sender;
		enabled = true;
		trans = transform;
		spawnTime = Time.time;
		if (mode == Mode.RigidBody)
		{
			rigid.isKinematic = false;
			rigid.AddForce(trans.forward * speed * rigid.mass, ForceMode.Impulse);
		}
		else 
		{
			mode = Mode.Simple;
		}
		Spawner.Recycle(gameObject, lifeTime);

	}

	public virtual void Hit() 
	{
		Recycle();
	}

	public void Recycle()
	{
		enabled = false;
		rigid.isKinematic = true;
		Spawner.Recycle(gameObject);
	}
}

