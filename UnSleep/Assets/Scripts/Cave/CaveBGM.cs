using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveBGM : MonoBehaviour
{
    public AudioClip caveBgm;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.FadeInBGM(caveBgm, 0.1f);
    }

}
