using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioTrigger : MonoBehaviour
{
    public AudioClip SoundToPlay;
    public float volume;
    AudioSource audio;
    public bool alreadyPlayed = false;
    public BossArea bossarea;
    public GameObject effects;
    public GameObject uI;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void OnTriggerEnter()
    {
        if (!alreadyPlayed)
        {
            audio.PlayOneShot(SoundToPlay, volume);
            alreadyPlayed = true;
            bossarea.StartBossFight();
            effects.SetActive(true);
            uI.SetActive(true);
        }
    }
}
