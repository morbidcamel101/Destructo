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

	public bool IsLow {
		get { return currentHealth <= totalHealth * lowPercentage; }
	}

	public bool IsCritical {
		get { return currentHealth <= totalHealth * criticalPercentage; }
	}


    
    void OnImpact(Bullet bullet)
	{
		Impact(bullet.damage * bullet.strengthMultiplier);
	}

	public void Impact(float damage)
	{
		currentHealth = Mathf.Clamp(currentHealth - damage, 0f, totalHealth);

		if (currentHealth == 0)
		{
			dead = true;
			SendMessage("OnDeath", SendMessageOptions.RequireReceiver);
		}
		else if (IsCritical)
		{
			SendMessage("OnCriticalHealth", SendMessageOptions.RequireReceiver);
		}
		else if (IsLow)
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

