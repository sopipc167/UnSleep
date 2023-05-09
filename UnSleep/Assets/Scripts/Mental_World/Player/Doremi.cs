using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doremi : MonoBehaviour
{
    public AudioClip[] audioClips;
    public float effectInterval = 5f;

    private int pitch = 0;
    public bool isEffecting = false;
    private Coroutine coroutine;

    public void playDoremi()
    {
        if (isEffecting)
        {
            pitch++;
            if (coroutine != null) StopCoroutine(coroutine);

        }
        else {
            pitch = 0;
        }

        SoundManager.Instance.PlaySE(audioClips[pitch % audioClips.Length], 0.5f);
        coroutine = StartCoroutine(countingEffect());

    }

    IEnumerator countingEffect()
    {
        isEffecting = true;

        yield return new WaitForSeconds(effectInterval);

        isEffecting = false;

        yield return null;
    }

}
