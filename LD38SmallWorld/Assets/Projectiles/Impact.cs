using System;
using UnityEngine;

[AddComponentMenu("Small World/Impact")]
[RequireComponent(typeof(Collider))]
public class Impact: BehaviorBase
{
	public float duration = 3;


	private PhysicMaterial material;

	void Awake()
	{
		material = GetComponent<Collider> ().sharedMaterial;
	}

	private void SpawnEffect ( MaterialImpactManager.ImpactEffect effect, Vector3 position, Quaternion rotation)
	{
		if (effect == null || effect.effect == null)
			return;
			
		// Kick off effect and recycle
		var instance = Spawner.Spawn (effect.effect, true, position, rotation);
		Spawner.Recycle (instance, duration);
	}

	private void BulletImpact (Vector3 point, Vector3 normal, Bullet bullet)
	{
		var impactEffect = MaterialImpactManager.Instance.GetImpactEffect (material);
		SpawnEffect (impactEffect, point, Quaternion.Euler(normal));
		Debug.DrawRay (point, normal, Color.red);
		// TODO Kick off audio
		// NOTE: The bullet get send with the message - what's the last thing that goes through a characters head? -- BULLET!!
		SendMessage ("OnImpact", bullet, SendMessageOptions.DontRequireReceiver);
		bullet.Hit ();
	}


	void OnCollisionEnter(Collision collision)
	{
		var t = collision.transform;
		Bullet bullet;
		if (!(bullet = t.GetComponent<Bullet>()) || !bullet.enabled)
			return;

		BulletImpact (collision.contacts[0].point, collision.contacts[1].normal, bullet);
	}

	void OnTriggerEnter(Collider other)
	{
		var bullet = other.GetComponent<Bullet>();
		if (!bullet || !bullet.enabled)
			return;

		BulletImpact(bullet.transform.position, -bullet.transform.forward, bullet);
	}
}

