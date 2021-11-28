using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    Keyboard keyboard = Keyboard.current;
    public AudioMixer audioMixer;
    public GameObject menu;
    private bool isPaused;
    private bool started;
    public GameObject dialogSystem;
    public GameObject dialogSystemMarket;

    public Slider SFX;
    public Slider Music;

    private AudioSource[] audios;

    public void Start()
    {

        if (PlayerPrefs.GetFloat("Music") > 0)
        {
            Debug.Log("Music " + PlayerPrefs.GetFloat("Music", 100f));
            Music.value = PlayerPrefs.GetFloat("Music", 100f);
        }
        if (PlayerPrefs.GetFloat("SFX") > 0)
        {
            Debug.Log("SFX " + PlayerPrefs.GetFloat("SFX", 60f));
            SFX.value = PlayerPrefs.GetFloat("SFX",60f);
        }
    }

    public void SetVolume(float vol)
    {
        audioMixer.SetFloat("Volume", Mathf.Log10(vol) * 20);
        PlayerPrefs.SetFloat("Volume", Mathf.Log10(vol) * 20);
        PlayerPrefs.Save();
    }
    public void SetMusicVolume(float vol)
    {
        audioMixer.SetFloat("Music", Mathf.Log10(vol) * 20);
        PlayerPrefs.SetFloat("Music", Mathf.Log10(vol) * 20);
        PlayerPrefs.Save();
    }
    public void SetSFXVolume(float vol)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(vol) * 20);
        PlayerPrefs.SetFloat("SFX", Mathf.Log10(vol) * 20);
        PlayerPrefs.Save();
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    private void Update()
    {
        PauseMenuToggle();
        PlayerAttackPause();
    }

    private void PauseMenuToggle()
    {
        if (keyboard.escapeKey.wasPressedThisFrame && !GameManager.Instance.Player.GetComponent<Animator>().GetBool("playerDead") && DialogSystemActive() == false && DialogSystemMarketActive() == false)
        {
            isPaused = !isPaused;
        }

        if (isPaused)
        {
            ApplicationInteracter.CursorModeNormal();
            PauseGame();
        }
        else if (!GameManager.Instance.Player.GetComponent<Animator>().GetBool("playerDead"))
        {
            ApplicationInteracter.CursorModeLocked();
            UnpauseGame();
        }
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        menu.SetActive(true);
        //audios = FindObjectsOfType<AudioSource>();
        //if (started)
        //{
        //    foreach (AudioSource aud in audios)
        //    {
        //        aud.Pause();
        //    }

        //    PlayerPrefs.Save();
        //    started = false;
        //}
        //add other pause code here
    }
    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        menu.SetActive(false);
        //if (!started)
        //{
        //    if (menu.activeSelf)
        //    {
        //        audios = FindObjectsOfType<AudioSource>();
        //        foreach (AudioSource aud in audios)
        //        {
        //            aud.Play();
        //            // this currently makes everything play a million times over, this should just be a start stop. works for some audio but doesnt for others.  
        //            // i know this is gettin called every frame theres more commented out code. 
        //            // im not sure where the audio for music and fires for a start are getting 
        //            // basically everything thats a loop sound that doesnt pause with the aud.pause command above doesnt restart currently
        //        }
        //    }

        //    PlayerPrefs.Save();
        //    started = true;
        //}

        isPaused = false;
        //add other unpause code here
    }

    public void ResetLevel()
    {
        SetPrefs();
        ApplicationInteracter.ReloadScene();
        Time.timeScale = 1f;
    }
    public void ExitToMainMenu()
    {
        SetPrefs();
        ApplicationInteracter.ApplicationLoadLevel("Mainmenu");
    }
    public void ExitGame()
    {
        SetPrefs();
        ApplicationInteracter.ApplicationQuit();
    }

    public void PlayerAttackPause()
    {
        if (menu.activeInHierarchy == true)
        {
            GameManager.Instance.Player.GetComponent<Animator>().SetBool("Attack1", false);
            GameManager.Instance.Player.GetComponent<Animator>().SetBool("Attack2", false);
            GameManager.Instance.Player.GetComponent<Animator>().SetBool("Attack3", false);
        }
    }

    public bool DialogSystemActive()
    {
        bool isactive = false;

        if (dialogSystem != null)
        {
            if (dialogSystem.activeSelf == true)
            {
                isactive = true;
            }
            else
            {
                isactive = false;
            }
        }

        return isactive;
    }
    public bool DialogSystemMarketActive()
    {
        bool isactive = false;

        if (dialogSystemMarket != null)
        {
            if (dialogSystemMarket.activeSelf == true)
            {
                isactive = true;
            }
            else
            {
                isactive = false;
            }
        }

        return isactive;
    }

    private void SetPrefs()
    {
        PlayerPrefs.SetFloat("Music", Music.value);
        PlayerPrefs.SetFloat("SFX", SFX.value);
        PlayerPrefs.Save();
    }
}
