using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public Text scoreText;

    public AudioClip buttonSelectSound;

    private AudioSource audioSource;
    private float volLowRange = .5f;
    private float volHighRange = 1.0f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        SetScoreText();
    }

    private void SetScoreText()
    {
        if (scoreText != null)
            scoreText.text = string.Format("Score: {0}", GameStatsHolder.scoreTotal.ToString());
    }

    public void StartGameButton(string newGameLevel)
    {
        PlayButtonSelect();
        CharacterManager.Difficulty = Difficulty.Easy;
        SceneManager.LoadScene(newGameLevel);
    }

    public void NormalGameButton(string newGameLevel)
    {
        PlayButtonSelect();
        CharacterManager.Difficulty = Difficulty.Normal;
        SceneManager.LoadScene(newGameLevel);
    }

    public void ExtremeGameButton(string newGameLevel)
    {
        PlayButtonSelect();
        CharacterManager.Difficulty = Difficulty.Extreme;
        SceneManager.LoadScene(newGameLevel);
    }

    public void LoadGameButton(string loadGameLevel)
    {
        PlayButtonSelect();
        SceneManager.LoadScene(loadGameLevel);
    }

    public void RetryGameButton(string loadGameLevel)
    {
        PlayButtonSelect();
        SceneManager.LoadScene(loadGameLevel);
    }

    public void CreditsButton()
    {
        PlayButtonSelect();

        // Dev Team Credits Page
        SceneManager.LoadScene("Credits");
    }

    public void DevelopedByButton()
    {
        PlayButtonSelect();

        // Dev Team Website
        Application.OpenURL("https://ldjam.com/events/ludum-dare/38/destructo-island");
    }

    public void ControlsButton()
    {
        PlayButtonSelect();
        SceneManager.LoadScene("Controls");
    }

    public void MainMenuButton()
    {
        PlayButtonSelect();
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGameButton()
    {
        PlayButtonSelect();
        Application.Quit();
    }

    public void BackButton()
    {
        PlayButtonSelect();

        // Return to previous screen
        SceneManager.LoadScene("MainMenu");
    }

    private void PlayButtonSelect()
    {
        if (buttonSelectSound != null)
        {
            float vol = Random.Range(volLowRange, volHighRange);
            audioSource.PlayOneShot(buttonSelectSound, vol);
        }
    }
}
