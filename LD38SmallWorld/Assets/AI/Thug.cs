using UnityEngine;
using System.Collections;
using System;

[AddComponentMenu("Small World/Thug")]
[RequireComponent(typeof(Health))]
public class Thug : CharacterBase
{

	protected override void OnDeath ()
	{
		throw new NotImplementedException(); // TODO
	}

	protected override void OnCriticalHealth ()
	{
		// TODO - Tint with blood!
		Log("Crititcal health reached! -- DUCK");
	}

	protected override void OnLowHealth ()
	{
		// TODO - Health is low --- find cover

	}
}

