using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Impact))]
public sealed class Health: BehaviorBase
{
	public float totalHealth = 100f;
	public float currentHealth = 100f;
	public float regenerationRate = 5f; // 5 seconds to regenerate
	public float criticalPercentage = 0.2f;
	public float lowPercentage = 0.5f;
	public bool dead = false;
    
    void OnImpact(Bullet bullet)
	{
		Impact(bullet.damage);
	}

	public void Impact(float damage)
	{
		currentHealth = Mathf.Clamp(currentHealth - damage, 0f, totalHealth);

		var critical = totalHealth * criticalPercentage;
		var low = totalHealth * lowPercentage;
		if (currentHealth == 0)
		{
			dead = true;
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

	void Update()
	{
		if (currentHealth == totalHealth)
			return;

		if (!dead)
		{
			var delta = regenerationRate * Time.deltaTime;
			currentHealth = Mathf.Clamp(currentHealth + delta, 0f, totalHealth); 
		}

	}

	public void Reset() 
	{
		currentHealth = totalHealth;
		dead = false;
	}
}

