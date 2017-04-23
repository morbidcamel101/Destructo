using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthMeterManager : MonoBehaviour
{
    public float totalHealth = 100f;
    public float currentHealth = 100f;
    public float regenerationRate = 5f;
    public float criticalPercentage = 0.2f;
    public float lowPercentage = 0.5f;
    public bool dead = false;

    public Image content;
    public GameObject[] bloodOverlayObjects;

    private bool showBloodOverlay = false;

	// Use this for initialization
	void Start ()
    {
        currentHealth = 100f;
        StartCoroutine(HandleRegenerationFactor());
    }
	
	// Update is called once per frame
	void Update ()
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
}
