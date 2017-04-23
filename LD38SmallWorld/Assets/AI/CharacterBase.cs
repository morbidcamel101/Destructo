using System;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public abstract class CharacterBase: BehaviorBase
{	
	// Fired from Health comp
	protected abstract void OnDeath();

	// Fired from Impact comp
	protected abstract void OnImpact(Bullet bullet);

	// Fired from Health comp
	protected abstract void OnCriticalHealth();

	protected abstract void OnLowHealth();

	public Holdster[] holdsters; // Desscribe the guns

	public Gun GetGun(string id)
	{
		var holdster =  holdsters.FirstOrDefault(h => h.id == id);
		if (holdster == null)
			return null;

		return holdster.gun;
	}

	public Health Health { get { return this.GetComponent<Health>(); } }

	public Player Player {
		get 
		{ 
			var obj = GameObject.FindGameObjectWithTag("Player"); 

			return obj != null ? obj.GetComponent<Player>() : default(Player);
		}
	}

}


