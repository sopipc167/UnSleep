using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveOtherSound : MonoBehaviour
{
    AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Walk_Sound()
    {
        audioSource.Play();
    }

}
