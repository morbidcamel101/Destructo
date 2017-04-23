using System;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

[AddComponentMenu("Small World/Player")]
[RequireComponent(typeof(Health))]
public class Player: CharacterBase
{

	protected override void OnDeath ()
	{
		// GAME OVER!!
		// TODO - Handle death

		this.GetComponent<FirstPersonController>().enabled = false;
		Log("YOU ARE DEAD! GAME OVER!");

	}

	protected override void OnCriticalHealth ()
	{
		Log("YOU HAVE CRITICAL HEALTH!");
		// TODO - Show Blood overlay
		
	}

	protected override void OnLowHealth ()
	{
		Log("HEALTH IS LOW!!");
	}

	protected override void OnImpact (Bullet bullet)
	{
		throw new NotImplementedException ();
	}
}


