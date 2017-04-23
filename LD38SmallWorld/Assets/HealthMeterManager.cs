using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthMeterManager : UIBehavior
{
    public float totalHealth
    {
        get
        {
            return Player.Health.totalHealth;
        }
    }

    public float currentHealth
    {
        get
        {
            return Player.Health.currentHealth;
        }
    }

    public float regenerationRate
    {
        get
        {
            return Player.Health.regenerationRate;
        }
    }

    public float criticalPercentage
    {
        get
        {
            return Player.Health.criticalPercentage;
        }
    }

    public float lowPercentage
    {
        get
        {
            return Player.Health.lowPercentage;
        }
    }

    public bool dead
    {
        get
        {
            return Player.Health.dead;
        }
    }

    public Image content;
    public GameObject[] bloodOverlayObjects;

    private bool showBloodOverlay = false;

	// Use this for initialization
	void Start ()
    {
        Player.Health.currentHealth = 100f;
        //StartCoroutine(HandleRegenerationFactor());
    }
	
	// Update is called once per frame
	protected override void UpdateUI ()
    {
        HandleHealthMeterDisplay();
    }

    private void HandleHealthMeterDisplay()
    {
        content.fillAmount = HealthFillAmount(currentHealth, 0, totalHealth, 0, 1);

        if (content.fillAmount <= lowPercentage && content.fillAmount > criticalPercentage)
        {
            content.color = Color.yellow;
            showBloodOverlay = false;
        }
        else if (content.fillAmount <= criticalPercentage)
        {
            content.color = Color.red;
            showBloodOverlay = true;
        }
        else
        {
            content.color = Color.green;
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
                Player.Health.currentHealth += 1; 
                
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
        Player.Health.currentHealth = Player.Health.currentHealth + 5;

        if (currentHealth > totalHealth)
            Player.Health.currentHealth = totalHealth;
    }

    public void HealthDecrease()
    {
        // Test
        Player.Health.currentHealth = Player.Health.currentHealth - 5;

        if (currentHealth < 0)
            Player.Health.currentHealth = 0;
    }
}
