using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using System.Collections.Generic;


public abstract class CharacterBase: BehaviorBase
{	
	// Fired from Health comp
	protected abstract void OnDeath();

	// Fired from Impact comp
	protected abstract void OnImpact(Bullet bullet);

	protected abstract void CanImpact(Bullet bullet);

	// Fired from Health comp
	protected abstract void OnCriticalHealth();

	protected abstract void OnLowHealth();

	public virtual void Ressurect()
	{
		
	}

	public Holdster[] holdsters; // Desscribe the guns


	public Gun GetGun(string id)
	{
		var holdster =  holdsters.FirstOrDefault(h => h.id == id);
		if (holdster == null)
			return null;

		return holdster.gun;
	}

	public virtual void SetTarget(Vector3 target)
	{
		foreach(var hold in holdsters)
		{
			hold.gun.SetTarget(target);
		}
	}

	public void Fire(string id = null)
	{
		if (holdsters.Length == 0)
			return;

		var gun = id != null ? GetGun(id) : holdsters[0].gun;
		gun.Fire();
	}

	public void StopFire(string id = null)
	{
		if (holdsters.Length == 0)
			return;

		var gun = id != null ? GetGun(id) : holdsters[0].gun;
		gun.StopFire();
	}

	public void FireAt(Transform target, string id = null)
	{
		FireAt(target.position, id);
	}

	public void FireAt(Vector3 position, string id = null)
	{
		if (holdsters.Length == 0)
			return;

		var gun = id != null ? GetGun(id) : holdsters[0].gun;
		gun.FireAt(position);
	}

	public Health Health { get { return this.GetComponent<Health>(); } }

	protected virtual void OnFire(Bullet bullet)
	{
		// Bullet is sent!!
		bullet.sender = this; // From Russia with love :)


	}

	

}


