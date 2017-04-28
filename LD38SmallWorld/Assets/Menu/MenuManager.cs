using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    #region Properties
    
    public Button selectedButton;
    
    #endregion

    #region Methods

    // Use this for initialization
    void Start ()
    {
        HighlightSelectedButton();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            Button prevButton = (Button)selectedButton.FindSelectableOnUp();
            if (prevButton != null)
            {
                selectedButton = prevButton;
                selectedButton.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            Button nextButton = (Button)selectedButton.FindSelectableOnDown();
            if (nextButton != null)
            {
                selectedButton = nextButton;
                selectedButton.Select();
            }
        }
        else if (Input.GetKeyUp(KeyCode.Return))
        {
            TriggerButtonClick();
        }
    }

    private void HighlightSelectedButton()
    {
        if (selectedButton != null)
        {
            selectedButton.Select();
        }

    }

    private void TriggerButtonClick()
    {
        if (selectedButton != null)
        {
            selectedButton.onClick.Invoke();
        }
    }

    #endregion
}
