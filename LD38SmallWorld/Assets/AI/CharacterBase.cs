using System;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public abstract class CharacterBase: BehaviorBase
{
	// Fired from Health
	protected abstract void OnDeath();

	// Fired from Health
	protected abstract void OnCriticalHealth();

	protected abstract void OnLowHealth();

}


