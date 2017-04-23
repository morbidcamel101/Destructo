using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthMeterManager : UIBehavior
{
    #region Properties

    public float totalHealth
    {
        get
        {
            if (Player != null)
                return Player.Health.totalHealth;
            else
                return 0;
        }
    }

    public float currentHealth
    {
        get
        {
            if (Player != null)
                return Player.Health.currentHealth;
            else
                return 0;
        }
    }

    public float regenerationDuration
    {
        get
        {
            if (Player != null)
                return Player.Health.regenerationDuration;
            else
                return 0;
        }
    }

    public float criticalPercentage
    {
        get
        {
            if (Player != null)
                return Player.Health.criticalPercentage;
            else
                return 0;
        }
    }

    public float lowPercentage
    {
        get
        {
            if (Player != null)
                return Player.Health.lowPercentage;
            else
                return 0;
        }
    }

    public bool dead
    {
        get
        {
            if (Player != null)
                return Player.Health.dead;
            else
                return false;
        }
    }

    public Image healthBar;
    public GameObject[] criticalHealthOverlayObjects;

    private bool showBloodOverlay = false;

    #endregion

    #region Methods

    // Use this for initialization
    void Start ()
    {
        if (Player != null)
        {
            Player.Health.currentHealth = 100f;
        }

        //StartCoroutine(HandleRegenerationFactor());
    }
	
	// Update is called once per frame
	protected override void UpdateUI ()
    {
        HandleHealthMeterDisplay();
    }

    private void HandleHealthMeterDisplay()
    {
        if (healthBar != null)
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

            HandleCriticalHealthOverlay();
        }
    }

    private void HandleCriticalHealthOverlay()
    {
        foreach (GameObject go in criticalHealthOverlayObjects)
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
        if (Player != null)
        {
            // Test
            Player.Health.currentHealth = Player.Health.currentHealth + 5;

            if (currentHealth > totalHealth)
                Player.Health.currentHealth = totalHealth;
        }
    }

    public void HealthDecrease()
    {
        if (Player != null)
        {
            // Test
            Player.Health.currentHealth = Player.Health.currentHealth - 5;

            if (currentHealth < 0)
                Player.Health.currentHealth = 0;
        }
    }

    #endregion
}
