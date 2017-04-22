using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoMeterManager : MonoBehaviour
{
    private float fillAmount;

    public Image content;

    // Use this for initialization
    void Start()
    {
        fillAmount = 100;
    }

    // Update is called once per frame
    void Update()
    {
        HandleAmmoMeterDisplay();
    }

    private void HandleAmmoMeterDisplay()
    {
        content.fillAmount = AmmoFillAmount(fillAmount, 0, 100, 0, 1);
    }

    private float AmmoFillAmount(float healthVal, float inMinHealthVal, float inMaxHealthVal, float outMinFillVal, float outMaxFillVal)
    {
        return (healthVal - inMinHealthVal) * (outMaxFillVal - outMinFillVal) / (inMaxHealthVal - inMinHealthVal) + outMinFillVal;
    }

    public void AmmoIncrease()
    {
        // Test relaod back to full capacity
        fillAmount = 100;
    }

    public void AmmoDecrease()
    {
        // Test
        fillAmount = fillAmount - 2;

        if (fillAmount < 0)
            fillAmount = 0;
    }
}
