using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepEnemies : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] clips;
    [SerializeField]
    private AudioClip arrowShotSound;
    [SerializeField]
    private AudioClip[] damageClips;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Step()
    {
        AudioClip clip = GetRandomClip();
        audioSource.PlayOneShot(clip);
    }

    private void ArrowShotSound()
    {
        AudioClip clip = arrowShotSound;
        audioSource.PlayOneShot(clip);
    }

    private void DamageSounds()
    {
        AudioClip clip = GetRandomDamageClip();
        audioSource.PlayOneShot(clip);
    }

    private AudioClip GetRandomDamageClip()
    {
        return damageClips[UnityEngine.Random.Range(0, damageClips.Length)];
    }

    private AudioClip GetRandomClip()
    {
        return clips[UnityEngine.Random.Range(0, clips.Length)];
    }
}
