using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Dropdown resolutionsDropdown;

    public Slider SFX;
    public Slider Music;

    Resolution[] resolutions;


     void Start()
    {
       resolutions = Screen.resolutions;

        resolutionsDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentReslutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentReslutionIndex = i;
            }
        }

        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = currentReslutionIndex;
        resolutionsDropdown.RefreshShownValue();

        if (PlayerPrefs.GetFloat("Music") > 0)
        {
            Debug.Log("Music " + PlayerPrefs.GetFloat("Music", 100f));
            Music.value = PlayerPrefs.GetFloat("Music", 100f);
        }
        if (PlayerPrefs.GetFloat("SFX") > 0)
        {
            Debug.Log("SFX " + PlayerPrefs.GetFloat("SFX", 60f));
            SFX.value = PlayerPrefs.GetFloat("SFX", 60f);
        }

    }


     public void SetVolume(float vol)
     {
         audioMixer.SetFloat("Volume", Mathf.Log10(vol) * 20);
         PlayerPrefs.SetFloat("Volume", Mathf.Log10(vol) * 20);
     }
     public void SetMusicVolume(float vol)
     {
         audioMixer.SetFloat("Music", Mathf.Log10(vol) * 20);
         PlayerPrefs.SetFloat("Music", Mathf.Log10(vol) * 20);
     }
     public void SetSFXVolume(float vol)
     {
         audioMixer.SetFloat("SFX", Mathf.Log10(vol) * 20);
         PlayerPrefs.SetFloat("SFX", Mathf.Log10(vol) * 20);
     }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void Back()
    {
        SetPrefs();
        ApplicationInteracter.ApplicationLoadLevel("MainMenu");
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void SetPrefs()
    {
        PlayerPrefs.SetFloat("Music", Music.value);
        PlayerPrefs.SetFloat("SFX", SFX.value);
        PlayerPrefs.Save();
    }
}
