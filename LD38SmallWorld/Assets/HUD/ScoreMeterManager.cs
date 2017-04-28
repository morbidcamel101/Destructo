using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMeterManager : UIBehavior
{
    #region Properties

    public float droneCount
    {
        get
        {
            if (CharacterManager.Instance != null)
                return CharacterManager.Instance.population;
            else
                return 0;
        }
    }

    public float score
    {
        get
        {
            if (Player != null)
                return Player.score;
            else
                return 0;
        }
    }

    public Text droneText;
    public Text scoreText;
    public Image waveCompletedImage;

    #endregion

    #region Methods

    // Use this for initialization
    void Start ()
    {
        droneText.text = "0";
        scoreText.text = "0";
	}

    // Update is called once per frame
    protected override void UpdateUI()
    {
        SetDroneText();
        SetScoreText();
    }

    private void SetDroneText()
    {
        if (droneText != null)
            droneText.text = string.Format("DRONES: {0}", Convert.ToInt32(droneCount));

        if (waveCompletedImage != null)
        {
            if (droneCount <= 0)
            {
                // Display wave completed
                waveCompletedImage.enabled = true;
            }
            else
            {
                // Next wave has started
                waveCompletedImage.enabled = false;
            }
        }
    }

    private void SetScoreText()
    {
        if (scoreText != null)
            scoreText.text = string.Format("SCORE: {0}", Convert.ToInt32(score));
    }

    #endregion
}
