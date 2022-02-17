using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLeftRight : MonoBehaviour
{
    public GameObject left_speaker;
    public GameObject right_speaker;

    AudioSource leftAudio;
    AudioSource rightAudio;

    public AudioClip[] audioClips; //나중에는 오브젝트 매니저 만들어서 에피별로 로드해서 사용

    //확인용
    public string position;
    public int idx;
    public float vol;

    void Awake()
    {
        leftAudio = left_speaker.GetComponent<AudioSource>();
        rightAudio = right_speaker.GetComponent<AudioSource>();

    }

    public void SetAudio(string SP,int SI, float volume)
    {
        position = SP;
        idx = SI;
        vol = volume;

        if (SP.Equals("C"))
        {
            leftAudio.clip = audioClips[idx];
            rightAudio.clip = audioClips[idx];
            leftAudio.Play();
            rightAudio.Play();

            if (leftAudio.mute)
                leftAudio.mute = false;

            if (rightAudio.mute)
                rightAudio.mute = false;

            leftAudio.volume = volume;
            rightAudio.volume = volume;
        }
        else if (SP.Equals("L"))
        {
            leftAudio.clip = audioClips[idx];
            leftAudio.Play();

            if (leftAudio.mute)
                leftAudio.mute = false;

            if (!rightAudio.mute)
                rightAudio.mute = true;

            leftAudio.volume = volume;
        }
        else if (SP.Equals("R"))
        {
            rightAudio.clip = audioClips[idx];
            rightAudio.Play();

            if (!leftAudio.mute)
                leftAudio.mute = true;

            if (rightAudio.mute)
                rightAudio.mute = false;

            rightAudio.volume = volume;

        }
    }


    public void SetAudioLR(int i1, float vol1, int i2, float vol2)
    {
        if (leftAudio.mute)
            leftAudio.mute = false;

        leftAudio.clip = audioClips[i1];
        leftAudio.volume = vol1;
        leftAudio.Play();

        if (rightAudio.mute)
            rightAudio.mute = false;

        rightAudio.clip = audioClips[i2];
        rightAudio.volume = vol2;
        rightAudio.Play();

    }

    public void SetAudioMute()
    {
        if (!leftAudio.mute)
            leftAudio.mute = true;

        if (!rightAudio.mute)
            rightAudio.mute = true;

    }

}
