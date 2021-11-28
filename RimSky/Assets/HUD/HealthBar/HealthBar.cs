using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip;
    public Slider slider;
    public Text healthPercentage;

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        healthPercentage.text = health.ToString();
    }
    public void PlayHealthSound()
    {
        audioSource.PlayOneShot(clip);
    }
    public void SetHealth(int health)
    {
        slider.value = health;
        healthPercentage.text = health.ToString();
       
    }
    public int GetHealth()
    {
        return (int)slider.value;
    }
}
