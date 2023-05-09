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
            case 5: SoundManager.Instance.PlaySE(se19); break;
            case 6: SoundManager.Instance.PlaySE(se20); break;
            case 11: SoundManager.Instance.PlaySE(se27); break;
            case 15: SoundManager.Instance.PlaySE(se50); break;
            case 17: SoundManager.Instance.PlaySE(se65); break;
        }
        
    }
}
