using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthMeterManager : MonoBehaviour
{
    private float fillAmount;
    
    public Image content;

	// Use this for initialization
	void Start ()
    {
        fillAmount = 100;
	}
	
	// Update is called once per frame
	void Update ()
    {
        HandleHealthMeterDisplay();
    }

    private void HandleHealthMeterDisplay()
    {
        content.fillAmount = HealthFillAmount(fillAmount, 0, 100, 0, 1);
    }

    private float HealthFillAmount(float healthVal, float inMinHealthVal, float inMaxHealthVal, float outMinFillVal, float outMaxFillVal)
    {
        return (healthVal - inMinHealthVal) * (outMaxFillVal - outMinFillVal) / (inMaxHealthVal - inMinHealthVal) + outMinFillVal;
    }

    public void HealthIncrease()
    {
        // Test
        fillAmount = fillAmount + 5;

        if (fillAmount > 100)
            fillAmount = 100;
    }

    public void HealthDecrease()
    {
        // Test
        fillAmount = fillAmount - 5;

        if (fillAmount < 0)
            fillAmount = 0;
    }
}
