using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveEffect : MonoBehaviour, IEffect
{
    public AudioClip se19;
    public AudioClip se20;
    public AudioClip se27;
    public AudioClip se50;
    public AudioClip se65;

    public void OnEffect()
    {
        switch (Dialogue_Proceeder.instance.CurrentEpiID)
        {
            case 19: SoundManager.Instance.PlaySE(se19); break;
            case 20: SoundManager.Instance.PlaySE(se20); break;
            case 27: SoundManager.Instance.PlaySE(se27); break;
            case 50: SoundManager.Instance.PlaySE(se50); break;
            case 65: SoundManager.Instance.PlaySE(se65); break;
        }
        
    }
}
