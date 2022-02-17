using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSound : MonoBehaviour
{

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayBombSE()
    {
        audioSource.Play();
    } 
 
}
