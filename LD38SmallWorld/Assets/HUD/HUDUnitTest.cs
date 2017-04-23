using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDUnitTest : MonoBehaviour
{
    #region Score Properties

    public Text scoreText;
    private int score = 0;

    #endregion

    #region Health Properties

    public float totalHealth = 100f;
    public float currentHealth = 100f;
    public float regenerationRate = 5f;
    public float criticalPercentage = 0.2f;
    public float lowPercentage = 0.5f;
    public bool dead = false;

    public Image healthBar;
    public GameObject[] criticalHealthOverlayObjects;
    private bool showBloodOverlay = false;

    #endregion

    #region Ammo Properties

    public float totalAmmo = 200f;
    public float currentAmmo = 200f;

    public Image ammoBar;

    #endregion

    public void Start()
    {
        score = 0;
        SetScoreText();

        currentHealth = 100f;
        StartCoroutine(HandleRegenerationFactor());

        currentAmmo = 200;
    }

    // Update is called once per frame
    public void Update()
    {
        HandleHealthMeterDisplay();
        HandleAmmoMeterDisplay();
    }

    #region Score Meter Unit Test

    public void ScoreIncrease()
    {
        score = score + 1000;
        SetScoreText();    
    }

    public void ScoreDecrease()
    {
        score = score - 1000;
        if (score <= 0)
        {
            // Nothing to update
            score = 0;
        }

        SetScoreText();
    }

    private void SetScoreText()
    {
        if (scoreText != null)
            scoreText.text = string.Format("Score: {0}", Convert.ToInt32(score));
    }

    #endregion

    #region Health Meter Unit Test

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

    #endregion

    #region Ammo Unit Test
    
    private void HandleAmmoMeterDisplay()
    {
        if (ammoBar != null)
        {
            ammoBar.fillAmount = AmmoFillAmount(currentAmmo, 0, totalAmmo, 0, 1);

            if (ammoBar.fillAmount <= criticalPercentage)
            {
                // Display Reload text
            }
        }
    }

    private float AmmoFillAmount(float ammoVal, float inMinAmmoVal, float inMaxAmmoVal, float outMinFillVal, float outMaxFillVal)
    {
        return (ammoVal - inMinAmmoVal) * (outMaxFillVal - outMinFillVal) / (inMaxAmmoVal - inMinAmmoVal) + outMinFillVal;
    }

    public void AmmoIncrease()
    {
        // Test relaod back to full capacity
        currentAmmo = totalAmmo;
    }

    public void AmmoDecrease()
    {
        // Test
        currentAmmo = currentAmmo - 2;

        if (currentAmmo < 0)
            currentAmmo = 0;
    }

    #endregion
}
