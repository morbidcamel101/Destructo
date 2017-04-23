using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void StartGameButton(string newGameLevel)
    {
        SceneManager.LoadScene(newGameLevel);
    }

    public void LoadGameButton(string loadGameLevel)
    {
        SceneManager.LoadScene(loadGameLevel);
    }

    public void RetryGameButton(string loadGameLevel)
    {
        SceneManager.LoadScene(loadGameLevel);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }

    public void CreditsButton(string loadGameLevel)
    {
        // Dev Team Credits Page
        SceneManager.LoadScene("Credits");
    }

}
