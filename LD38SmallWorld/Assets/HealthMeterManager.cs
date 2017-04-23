using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthMeterManager : MonoBehaviour
{
    public float totalHealth = 100f;
    public float currentHealth = 100f;
    public float regenerationRate = 1f;
    public float criticalPercentage = 0.2f;
    public float lowPercentage = 0.5f;
    public bool dead = false;

    public Image content;

	// Use this for initialization
	void Start ()
    {
        currentHealth = 100f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        HandleHealthMeterDisplay();
        HandleRegenrationFactor();
    }

    private void HandleHealthMeterDisplay()
    {
        content.fillAmount = HealthFillAmount(currentHealth, 0, totalHealth, 0, 1);

        if (content.fillAmount <= lowPercentage && content.fillAmount > criticalPercentage)
        {
            content.color = Color.yellow;
        }
        else if (content.fillAmount <= criticalPercentage)
        {
            content.color = Color.red;
        }
        else
        {
            content.color = Color.green;
        }
    }

    private void HandleRegenrationFactor()
    {
        //currentHealth = currentHealth + regenerationRate;
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
}
