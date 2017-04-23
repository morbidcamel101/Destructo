using System;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Impact))]
public sealed class Health: BehaviorBase
{
	public float totalHealth = 100f;
	public float currentHealth = 100f;
	public float regenerationRate = 5f;
	public float criticalPercentage = 0.2f;
	public float lowPercentage = 0.5f;
	public bool dead = false;

	void Awake()
	{
		enabled = false;
	}

	void OnEnable()
	{
		StartCoroutine(Regenerate());
	}

	void OnImpact(Bullet bullet)
	{
		currentHealth = Mathf.Clamp(currentHealth - bullet.damage, 0f, totalHealth);

		var critical = totalHealth * criticalPercentage;
		var low = totalHealth * lowPercentage;
		if (currentHealth == 0)
		{
			SendMessage("OnDeath", SendMessageOptions.RequireReceiver);
		}
		else if (currentHealth <= critical)
		{
			SendMessage("OnCriticalHealth", SendMessageOptions.RequireReceiver);
		}
		else if (currentHealth <= low)
		{
			SendMessage("OnLowHealth", SendMessageOptions.RequireReceiver);
		}

	}

	IEnumerator Regenerate()
	{
		if (currentHealth == totalHealth)
			yield break;

		

		currentHealth += regenerationRate;
		yield return new WaitForSeconds(regenerationRate);
		
	}


}

