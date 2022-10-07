using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Epi7Sound : MonoBehaviour
{
    public AudioClip[] audioClip;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (Dialogue_Proceeder.instance.CurrentDiaID == 2008)
            audioSource.clip = audioClip[0];
        else if (Dialogue_Proceeder.instance.CurrentDiaID == 2014)
            audioSource.clip = audioClip[1];

        audioSource.Play();
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
