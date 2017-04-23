using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Impact))]
public sealed class Health: BehaviorBase
{
	public float totalHealth = 100f;
	public float currentHealth = 100f;
	public float regenerationDuration = 15f; // 5 seconds to regenerate
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

		var t = 1f/regenerationDuration;
		currentHealth = Mathf.Clamp(currentHealth + (totalHealth * t), 0f, totalHealth); 

		if (currentHealth == totalHealth)
			yield break;

		yield return new WaitForSeconds(t);
	}

	public void Reset() 
	{
		currentHealth = totalHealth;
		dead = false;
	}
}

