﻿using System;
using UnityEngine;

[AddComponentMenu("Small World/Impact")]
public class Impact: BehaviorBase
{
	public Prototype effect;
	public float duration = 3;

	void Awake()
	{
		Ensure(effect.prefab);
	}

	void OnTriggerEnter(Collider other) {

		Log("Impact -> "+other.gameObject.ToString());
		var bullet = other.GetComponent<Bullet>();
		if (bullet == null)
			return;

		bullet.enabled = false;
	}

	void OnCollisionEnter(Collision collision)
	{
		var t = collision.transform;
		Bullet bullet;
		if (!(bullet = t.GetComponent<Bullet>()))
			return;

		foreach(var contact in collision.contacts)
		{
			Debug.DrawRay(contact.point, contact.normal, Color.red);

			// Kick off effect and recycle
			var instance = Spawner.Spawn(effect, true, contact.point, Quaternion.Euler(-bullet.transform.forward));
			Spawner.Recycle(instance, duration);
		}

		// TODO Kick off audio



	}
}
