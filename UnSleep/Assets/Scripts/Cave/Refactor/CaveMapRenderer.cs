using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveMapRenderer : MonoBehaviour
{
    public SpriteRenderer background;
    public Sprite[] backgroundSprites;

    public GameObject[] holes;

    public AudioSource leftAudio;
    public AudioSource rightAudio;
    public AudioClip[] audioClips;


    public void renderCavern(Cavern cavern)
    {
        if (cavern.routeCnt < 0) 
            return; // 오류 방지 early return
        else if (cavern.routeCnt == 999)
        {
            background.sprite = backgroundSprites[4]; // 마지막 방 배경
            StartCoroutine(fadeOutCavernAudio());
            return;
        }
        

        background.sprite = backgroundSprites[cavern.routeCnt];
        
        foreach(GameObject h in holes)
        {
            h.SetActive(false);
        }

        holes[cavern.routeCnt].SetActive(true);

        switch (cavern.soundPosition)
        {
            case "LR":
                setCarvenAudioLR(cavern.soundIndex, cavern.volume, cavern.soundIndex2, cavern.volume2);
                break;
            case "C":
                setCavernAudio(leftAudio, cavern.soundIndex, cavern.volume, true);
                setCavernAudio(rightAudio, cavern.soundIndex, cavern.volume, true);
                break;
            case "L":
                setCavernAudio(leftAudio, cavern.soundIndex, cavern.volume, true);
                muteAudioSource(rightAudio);
                break;
            case "R":
                setCavernAudio(rightAudio, cavern.soundIndex, cavern.volume, true);
                muteAudioSource(leftAudio);
                break;
            case "c":
                setCavernAudio(leftAudio, cavern.soundIndex, cavern.volume, false);
                setCavernAudio(rightAudio, cavern.soundIndex, cavern.volume, false);
                break;
            case "l":
                setCavernAudio(leftAudio, cavern.soundIndex, cavern.volume, false);
                muteAudioSource(rightAudio);
                break;
            case "r":
                setCavernAudio(rightAudio, cavern.soundIndex, cavern.volume, false);
                muteAudioSource(leftAudio);
                break;
        }
    }

    private void setCavernAudio(AudioSource audioSource, int idx, float vol, bool first)
    {
        if (audioSource.mute)
            audioSource.mute = false;

        if (first)
        {
            audioSource.clip = audioClips[idx];
            audioSource.Play();
        }
           
        audioSource.volume = vol;
    }

    private void setCarvenAudioLR(int idx, float vol, int idx2, float vol2)
    {
        if (leftAudio.mute)
            leftAudio.mute = false;

        leftAudio.clip = audioClips[idx];
        leftAudio.volume = vol;


        if (rightAudio.mute)
            rightAudio.mute = false;

        rightAudio.clip = audioClips[idx2];
        rightAudio.volume = vol2;
    }

    private void muteAudioSource(AudioSource audioSource)
    {
        audioSource.mute = true;
    }

    IEnumerator fadeOutCavernAudio()
    {
        float rightvol = rightAudio.volume;
        float leftvol = leftAudio.volume;


        while (rightvol > 0f && leftvol > 0f)
        {

            rightvol -= 0.1f * Time.deltaTime;
            leftvol -= 0.1f * Time.deltaTime;

            rightAudio.volume = rightvol;
            leftAudio.volume = leftvol;

            yield return null;
        }
    }
}
