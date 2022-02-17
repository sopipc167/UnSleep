using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeControl : MonoBehaviour
{
    AudioSource audiosource;
    int id;
    // Start is called before the first frame update
    void Start()
    {
        audiosource = gameObject.GetComponent<AudioSource>();
        audiosource.mute = false;
        audiosource.loop = true;
        audiosource.volume = 0.3f;
        id = Random.Range(1, 4);
        Debug.Log(id);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setVolume(int buttonid)
    {
        if (buttonid == id)
            audiosource.volume += 0.2f;
        else
            audiosource.volume -= 0.2f;
        id = Random.Range(1, 4);
        Debug.Log(id);
    }
}
