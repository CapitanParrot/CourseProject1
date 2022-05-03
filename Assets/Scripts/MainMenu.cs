using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Отвечает за две кнопки из главного меню.
public class MainMenu : MonoBehaviour
{
    public string MainSceneName = "MainScene";
    public void PlayGame()
    {
        SceneManager.LoadScene(MainSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
