using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AmmoMeterManager : MonoBehaviour
{
    public float totalAmmo = 200f;
    public float currentAmmo = 200f;
    public float lowPercentage = 0.5f;
    public float criticalPercentage = 0.2f;

    public Image ammoBar;

    // Use this for initialization
    void Start()
    {
        currentAmmo = 200;
    }

    // Update is called once per frame
    void Update()
    {
        HandleAmmoMeterDisplay();
    }

    private void HandleAmmoMeterDisplay()
    {
        ammoBar.fillAmount = AmmoFillAmount(currentAmmo, 0, totalAmmo, 0, 1);

        if (ammoBar.fillAmount <= criticalPercentage)
        {
            // Display Reload text
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
}
