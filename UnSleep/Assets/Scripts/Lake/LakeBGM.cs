using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LakeBGM : MonoBehaviour
{

    public AudioClip lakeBgm;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.FadeInBGM(lakeBgm);
    }

}
