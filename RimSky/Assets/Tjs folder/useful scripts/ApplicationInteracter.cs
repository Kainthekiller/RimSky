using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ApplicationInteracter
{
    
    public static void ApplicationOpenURL(string _siteUrl)
    {
        Application.OpenURL(_siteUrl);
    }

    public static void ApplicationQuit()
    {
        Application.Quit();
    }

    public static void ApplicationLoadLevel(string _levelName)
    {
        SceneManager.LoadScene(_levelName);
    }

    public static void ApplicationLoadLevel(int _levelId)
    {
        SceneManager.LoadScene(_levelId);
    }

    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public static void CursorVisible(bool visible)
    {
        Cursor.visible = visible;
    }

    public static void CursorModeConfined()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public static void CursorModeLocked()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public static void CursorModeNormal()
    {
        Cursor.lockState = CursorLockMode.None;
    }


}
