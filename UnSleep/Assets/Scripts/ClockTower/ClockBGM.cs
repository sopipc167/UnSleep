using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockBGM : MonoBehaviour
{
    public AudioClip ctBgm;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.FadeInBGM(ctBgm);
    }

}
