using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Impact))]
public sealed class Health: BehaviorBase
{
	public float totalHealth = 100f;
	public float currentHealth = 100f;
	public float regenerationRate = 5f;
	public float criticalPercentage = 0.2f;
	public float lowPercentage = 0.5f;
	public bool dead = false;

    public Image healthBar;
    public GameObject[] bloodOverlayObjects;

    private bool showBloodOverlay = false;

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


    // Update is called once per frame
    void Update()
    {
        HandleHealthMeterDisplay();
    }

    private void HandleHealthMeterDisplay()
    {
        healthBar.fillAmount = HealthFillAmount(currentHealth, 0, totalHealth, 0, 1);

        if (healthBar.fillAmount <= lowPercentage && healthBar.fillAmount > criticalPercentage)
        {
            healthBar.color = Color.yellow;
            showBloodOverlay = false;
        }
        else if (healthBar.fillAmount <= criticalPercentage)
        {
            healthBar.color = Color.red;
            showBloodOverlay = true;
        }
        else
        {
            healthBar.color = Color.green;
            showBloodOverlay = false;
        }

        HandleBloodOverlay();
    }

    private void HandleBloodOverlay()
    {
        foreach (GameObject go in bloodOverlayObjects)
        {
            go.SetActive(showBloodOverlay);
        }
    }

    IEnumerator HandleRegenerationFactor()
    {
        while (true)
        {
            // Loops forever...
            if (currentHealth < totalHealth)
            {
                // If health < total max...
                currentHealth += 1;

                // Increase health and wait the specified time
                yield return new WaitForSeconds(1);
            }
            else
            {
                // If health >= total max, just yield 
                yield return null;
            }
        }
    }

    private float HealthFillAmount(float healthVal, float inMinHealthVal, float inMaxHealthVal, float outMinFillVal, float outMaxFillVal)
    {
        return (healthVal - inMinHealthVal) * (outMaxFillVal - outMinFillVal) / (inMaxHealthVal - inMinHealthVal) + outMinFillVal;
    }

    public void HealthIncrease()
    {
        // Test
        currentHealth = currentHealth + 5;

        if (currentHealth > totalHealth)
            currentHealth = totalHealth;
    }

    public void HealthDecrease()
    {
        // Test
        currentHealth = currentHealth - 5;

        if (currentHealth < 0)
            currentHealth = 0;
    }

    public void Reset()
    {
    	this.currentHealth = this.totalHealth;
    	this.dead = false;
    }
}

