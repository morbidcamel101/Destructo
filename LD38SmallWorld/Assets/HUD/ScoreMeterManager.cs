using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMeterManager : UIBehavior
{
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
            scoreText.text = string.Format("Score: {0}", Convert.ToInt32(score));
    }
}
