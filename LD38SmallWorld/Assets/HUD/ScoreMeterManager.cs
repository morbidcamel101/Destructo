using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMeterManager : UIBehavior
{
    #region Properties

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

    public Text scoreText;

    #endregion

    #region Methods

    // Use this for initialization
    void Start ()
    {
        scoreText.text = "0";
	}

    // Update is called once per frame
    protected override void UpdateUI()
    {
        SetScoreText();
    }
    
    private void SetScoreText()
    {
        if (scoreText != null)
            scoreText.text = string.Format("SCORE: {0}", Convert.ToInt32(score));
    }

    #endregion
}
