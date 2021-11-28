using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

public class ManaBar : MonoBehaviour
{
    public Slider slider;
    public Text manaPercentage;
    private GameObject player;
    public AudioSource audioSource;
    public AudioClip clip;

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    private void Start()
    {
        player = GameObject.Find("Player");
    }
    public void SetMaxMana(int mana)
    {
        slider.maxValue = mana;
        slider.value = mana;
        manaPercentage.text = mana.ToString();
    }

    public void SetMana(int mana)
    {
        slider.value = mana;
        manaPercentage.text = mana.ToString();
        player.GetComponent<ThirdPersonController>().CurrentPlayerMana = mana;
    }
    public int GetMana()
    {
        return (int)slider.value;
    }
    public void PlayManaSound()
    {
        audioSource.PlayOneShot(clip);
    }
}
