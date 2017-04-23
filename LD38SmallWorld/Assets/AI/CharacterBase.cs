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

	// Fired from Health comp
	protected abstract void OnCriticalHealth();

	protected abstract void OnLowHealth();

	public virtual void Randomize()
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

	public void SetTarget(Vector3 target)
	{
		foreach(var hold in holdsters)
		{
			hold.gun.SetTarget(target);
		}
	}

	public void Fire()
	{
		foreach(var hold in holdsters)
		{
			hold.gun.Fire();
		}
	}

	public void FireAt(Transform target)
	{
		FireAt(target.position);
	}

	public void FireAt(Vector3 position)
	{
		foreach(var hold in holdsters)
		{
			hold.gun.FireAt(position);
		}
	}

	public Health Health { get { return this.GetComponent<Health>(); } }

	

}


