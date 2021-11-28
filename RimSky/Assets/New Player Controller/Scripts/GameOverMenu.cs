using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverMenu;

    public void Update()
    {
        if (gameOverMenu.activeSelf)
        {
            ApplicationInteracter.CursorModeNormal();
        }
    }

    public void Restart()
    {
        ApplicationInteracter.ReloadScene();
    }

    public void MainMenu()
    {
        ApplicationInteracter.ApplicationLoadLevel("MainMenu");
    }
    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverMenu.SetActive(true);
        ApplicationInteracter.CursorModeNormal();
       // Debug.Log("Game Over Ran");
    }
}
