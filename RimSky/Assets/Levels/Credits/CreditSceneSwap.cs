using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditSceneSwap : MonoBehaviour
{

    public AudioMixer audioMixer;
    public float timer = 35;
    public Slider slider;
    private float SkipTimer = 3;
    public float startTime;
    private bool checkTime = false;

    // Start is called before the first frame update
    void Start()
    {
        ApplicationInteracter.CursorModeLocked();
        Time.timeScale = 1;

        audioMixer.SetFloat("Music", Mathf.Log10(PlayerPrefs.GetFloat("Music")) * 20);

    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            ToMainMenu();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            startTime = Time.time;
            checkTime = true;
        }


        if (Time.time - startTime >= SkipTimer && checkTime == true)
        {
            ToMainMenu();
        }

        if (Input.GetKeyUp(KeyCode.Space) && Time.time - startTime <= SkipTimer)
        {
            //Debug.Log((Time.time - startTime).ToString("00:00.00"));
            
            checkTime = false;

        }

        if (checkTime)
        {
            slider.value = (Time.time - startTime) * 33;
        }
        else
        {
            slider.value = 0;
        }

    }

    void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

   
}
