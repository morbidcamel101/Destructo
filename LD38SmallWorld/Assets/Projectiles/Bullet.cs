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

	private Transform trans;
	private float spawnTime;
	private Rigidbody rigid;

	void Awake()
	{
		enabled = false;
		rigid = GetComponent<Rigidbody>();
	}


	// Use this for initialization
	void OnEnable ()
	{
		trans = transform;
		spawnTime = Time.time;
		rigid.isKinematic = false;
		rigid.AddForce(trans.forward * speed * rigid.mass, ForceMode.Impulse);
	}

	void OnDisable() {
		rigid.isKinematic = true;
	}

	// Update is called once per frame
	void Update ()
	{
		trans.position += trans.forward * speed * Time.deltaTime;
		distance -= speed * Time.deltaTime;

		if ((Time.time > spawnTime + lifeTime || distance < 0) && Expired())
		{
			Expired();
			Spawner.Recycle(gameObject);
		}
		// Todo -- Hit
	
	}

	public virtual void Shoot()
	{
		enabled = true;
	}

	public virtual void Hit() 
	{
		enabled = false;
		throw new NotImplementedException();
	}

	public virtual bool Expired()
	{
		
		enabled = false;
		return true; // Yes - We expired
	}
}

