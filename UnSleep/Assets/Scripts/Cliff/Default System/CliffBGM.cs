using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CliffBGM : MonoBehaviour
{
    public AudioClip cliffBgm;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.FadeInBGM(cliffBgm);
    }

}
