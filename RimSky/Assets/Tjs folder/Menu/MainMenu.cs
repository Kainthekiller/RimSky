using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public AudioMixer audioMixer;

    public void Start()
    {
        ApplicationInteracter.CursorModeNormal();

        audioMixer.SetFloat("SFX", Mathf.Log10(PlayerPrefs.GetFloat("SFX")) * 20);
        
    }

    public void Play()
    {
        ApplicationInteracter.ApplicationLoadLevel("Terrain");
        
    }
    public void Credits()
    {
        ApplicationInteracter.ApplicationLoadLevel("Credits");
       
    }

    public void Settings()
    {
        ApplicationInteracter.ApplicationLoadLevel("SettingsMenu");
    }

    public void Exit()
    {
        ApplicationInteracter.ApplicationQuit();
    }
}
