using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwingSounds : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] clips;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip strongSwingClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void SwordSwing()
    {
        AudioClip clip = GetRandomClip();
        audioSource.PlayOneShot(clip);
    }
    private void StrongSwingSound()
    {
        audioSource.PlayOneShot(strongSwingClip);
    }

    private AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }
}
