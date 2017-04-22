using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDButtonTest : MonoBehaviour
{

    public Text scoreText;

    private int score = 0;

    public void Start()
    {
        score = 0;
        SetScoreText();
    }

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
}
