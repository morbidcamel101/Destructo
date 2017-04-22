using System;
using UnityEngine;

public abstract class UIBehavior: BehaviorBase
{
	public float updateInterval;

	private float resumeTime = 0f;


	void LateUpdate()
	{
		if (Time.time < resumeTime)
			return;

		UpdateUI();
	}

	protected abstract void UpdateUI();

}

