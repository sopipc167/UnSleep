using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClkSound : MonoBehaviour
{
    public AudioClip[] clips;
    public AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();    
    }

    public void PlaySound(int i)
    {
        audioSource.PlayOneShot(clips[i]);
    }



}
