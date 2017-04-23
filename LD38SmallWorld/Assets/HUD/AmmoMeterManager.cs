using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AmmoMeterManager : UIBehavior
{
    public Gun gun
    {
        get
        {
            if (Player != null)
            {
                return Player.GetGun("uzi");
            }
            else
                return null;
        }
    }

    public float totalAmmo
    {
        get
        {
            if (Player != null && gun != null)
            {
                return gun.clipSize;
            }
            else
                return 0;
        }
    }

    public float currentAmmo
    {
        get
        {
            if (Player != null && gun != null)
            {
                return gun.currentClip;
            }
            else
                return 0;
        }
    }

    public float lowPercentage = 0.5f;
    public float criticalPercentage = 0.2f;

    public Image ammoBar;
    public Text ammoText;

    #region Methods

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void UpdateUI()
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

        if (gun != null)
        {
            if (gun.state == Gun.State.Reloading)
            {
                ammoText.text = "Reloading...";
            }
            else if (gun.state == Gun.State.Reloaded)
            {
                ammoText.text = "";
                ammoBar.fillAmount = 1;
            }
        }
    }

    private float AmmoFillAmount(float ammoVal, float inMinAmmoVal, float inMaxAmmoVal, float outMinFillVal, float outMaxFillVal)
    {
        return (ammoVal - inMinAmmoVal) * (outMaxFillVal - outMinFillVal) / (inMaxAmmoVal - inMinAmmoVal) + outMinFillVal;
    }

    public void AmmoIncrease()
    {
        if (gun != null)
        {
            // Test relaod back to full capacity
            gun.currentClip = (int)totalAmmo;
        }
    }

    public void AmmoDecrease()
    {
        if (gun != null)
        {
            // Test
            gun.currentClip = gun.currentClip - 2;

            if (gun.currentClip < 0)
                gun.currentClip = 0;
        }
    }

    #endregion
}
