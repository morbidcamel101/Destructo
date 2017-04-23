using System;
using UnityEngine;

[AddComponentMenu("Small World/Impact")]
[RequireComponent(typeof(Collider))]
public class Impact: BehaviorBase
{
	public float duration = 3;


	void OnTriggerEnter(Collider other) {

		Log("Impact -> "+other.gameObject.ToString());
		var bullet = other.GetComponent<Bullet>();
		if (bullet == null)
			return;

		bullet.enabled = false;
	}

	private void SpawnEffect ( MaterialImpactManager.ImpactEffect effect, Vector3 position, Quaternion rotation)
	{
		if (effect == null || effect.effect == null)
			return;
			
		// Kick off effect and recycle
		var instance = Spawner.Spawn (effect.effect, true, position, rotation);
		Spawner.Recycle (instance, duration);
	}

	void OnCollisionEnter(Collision collision)
	{
		var t = collision.transform;
		Bullet bullet;
		if (!(bullet = t.GetComponent<Bullet>()))
			return;

		bullet.Hit();

		var material = GetComponent<Collider>().sharedMaterial;
		var impactEffect = MaterialImpactManager.Instance.GetImpactEffect(material);

		foreach(var contact in collision.contacts)
		{
			Debug.DrawRay(contact.point, contact.normal, Color.red);

			if (impactEffect != null)
				SpawnEffect (impactEffect, contact.point, Quaternion.Euler(-contact.normal));
		}

		// TODO Kick off audio
		// NOTE: The bullet get send with the message - what's the last thing that goes through a characters head? -- BULLET!!
		SendMessage("OnImpact", bullet, SendMessageOptions.DontRequireReceiver);
	}
}

