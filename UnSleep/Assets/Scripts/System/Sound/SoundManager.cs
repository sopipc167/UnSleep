﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType { Master, BGM, SE }

public class SoundManager : MonoBehaviour
{
    #region 싱글톤 클래스
    private static SoundManager instance;

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<SoundManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    SoundManager newObj = Resources.Load<SoundManager>("Singleton/Sound Manager");
                    instance = Instantiate(newObj);
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [Header("오디오 믹서")]
    public AudioMixer mainMixer;
    public AudioMixerGroup bgmMixGroup;
    public AudioMixerGroup seMixGroup;
    private bool[] isMuted = { false, false, false };

    //bgm
    private AudioSource bgmSource;
    private bool isChanging = false;

    //se
    private readonly Dictionary<string, AudioClip> seDic = new Dictionary<string, AudioClip>();
    private AudioSource seSource;

    void Start()
    {
        //bgm
        GameObject bgmObj = new GameObject("BGM Source", typeof(AudioSource));
        bgmObj.transform.SetParent(transform);
        bgmSource = bgmObj.GetComponent<AudioSource>();
        bgmSource.outputAudioMixerGroup = bgmMixGroup;
        bgmSource.loop = true;
        bgmSource.playOnAwake = true;

        //se
        GameObject seObj = new GameObject("SE Source", typeof(AudioSource));
        seObj.transform.SetParent(transform);
        seSource = seObj.GetComponent<AudioSource>();
        seSource.outputAudioMixerGroup = seMixGroup;
        seSource.loop = false;
        seSource.playOnAwake = false;
    }


    public void Clear()
    {
        seDic.Clear();
    }

    //BGM
    #region BGM

    public void PlayBGM(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        StopAllCoroutines();
        bgmSource.volume = volume;

        if (bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }

        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void PlayBGM(string name, float volume = 1f)
    {
        AudioClip currentClip = GetAudioClip(name, SoundType.BGM);
        if (currentClip == null) return;

        StopAllCoroutines();
        bgmSource.volume = volume;

        if (bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }

        bgmSource.clip = currentClip;
        bgmSource.Play();
    }

    public void ChangeBGM(AudioClip clip, float volume = 1f, float outDelay = 5f, float inDelay = 5f)
    {
        StopAllCoroutines();
        StartCoroutine(ChangeBGMCoroutine(clip, volume, outDelay, inDelay));
    }

    public void ChangeBGM(string name, float volume = 1f, float outDelay = 5f, float inDelay = 5f)
    {
        StopAllCoroutines();
        StartCoroutine(ChangeBGMCoroutine(name, volume, outDelay, inDelay));
    }

    public void PauseBGM()
    {
        if (bgmSource.isPlaying)
        {
            bgmSource.Pause();
        }
    }

    public void UnPauseBGM()
    {
        if (!bgmSource.isPlaying)
        {
            bgmSource.UnPause();
        }
    }

    public void FadeOutBGM(float volume = 0.001f, float delay = 5f)
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutBGMCoroutine(delay, volume));
    }

    public void FadeInBGM(AudioClip clip, float volume = 1f, float delay = 5f)
    {
        StopAllCoroutines();
        PlayBGM(clip, 0f);
        StartCoroutine(FadeInBGMCoroutine(delay, volume));
    }

    public void FadeInBGM(string name, float volume = 1f, float delay = 5f)
    {
        StopAllCoroutines();
        PlayBGM(name, 0f);
        StartCoroutine(FadeInBGMCoroutine(delay, volume));
    }

    #endregion


    //SE
    #region SE

    public void PlaySE(string name, float volume = 1f)
    {
        if (!seDic.TryGetValue(name, out AudioClip currentClip))
        {
            currentClip = GetAudioClip(name, SoundType.SE);
            if (currentClip == null) return;
            seDic[name] = currentClip;
        }

        seSource.PlayOneShot(currentClip, volume);
    }

    public void PlaySE(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        seSource.PlayOneShot(clip, volume);
    }

    public void StopSE()
    {
        seSource.Stop();
        //seStero.Stop();
    }

    #endregion


    //볼륨 조절 함수
    #region 볼륨 조절

    public void SetVolume(SoundType type, float volume)
    {
        if (isMuted[(int)type]) return;

        string parameter = string.Empty;
        switch (type)
        {
            case SoundType.Master:
                parameter = "Master";
                break;
            case SoundType.BGM:
                parameter = "BGM";
                break;
            case SoundType.SE:
                parameter = "SE";
                break;
            default:
                break;
        }
        if (volume < 0.001f) volume = 0.00000001f;
        mainMixer.SetFloat(parameter, Mathf.Log10(volume) * 20);
    }

    public void SetMute(SoundType type)
    {
        SetVolume(type, 0f);
        isMuted[(int)type] = true;
    }

    public void Unmute(SoundType type, float originVolume)
    {
        isMuted[(int)type] = false;
        SetVolume(type, originVolume);
    }

    #endregion


    //이후 함수는 외부에서 사용X
    #region 내부 코루틴

    private IEnumerator FadeOutBGMCoroutine(float delay, float volume = 0.001f)
    {
        isChanging = true;

        float tmp = 1f / delay;
        while (bgmSource.volume > volume)
        {
            bgmSource.volume -= Time.deltaTime * tmp;
            yield return null;
        }
        bgmSource.volume = volume;
        if (bgmSource.volume < 0.01f) bgmSource.volume = 0f;

        isChanging = false;
    }

    private IEnumerator FadeInBGMCoroutine(float delay, float volume = 1f)
    {
        isChanging = true;

        float tmp = volume / delay;
        bgmSource.volume = 0f;
        while (bgmSource.volume < volume)
        {
            bgmSource.volume += Time.deltaTime * tmp;
            yield return null;
        }
        bgmSource.volume = volume;

        isChanging = false;
    }

    private IEnumerator ChangeBGMCoroutine(string name, float volume, float outDelay, float inDelay)
    {
        StartCoroutine(FadeOutBGMCoroutine(outDelay));
        yield return new WaitUntil(() => !isChanging);
        PlayBGM(name, 0f);
        StartCoroutine(FadeInBGMCoroutine(inDelay, volume));
    }

    private IEnumerator ChangeBGMCoroutine(AudioClip clip, float volume, float outDelay, float inDelay)
    {
        StartCoroutine(FadeOutBGMCoroutine(outDelay));
        yield return new WaitUntil(() => !isChanging);
        PlayBGM(clip, 0f);
        StartCoroutine(FadeInBGMCoroutine(inDelay, volume));
    }

    #endregion

    private AudioClip GetAudioClip(string name, SoundType type)
    {
        AudioClip result = null;
        switch (type)
        {
            case SoundType.BGM:
                result = Resources.Load<AudioClip>("Sound/BGM/" + name);
                break;
            case SoundType.SE:
               if (!seDic.TryGetValue(name, out result))
                {
                    result = Resources.Load<AudioClip>("Sound/SE/" + name);
                    seDic[name] = result;
                }
                break;
            default:
                break;
        }

        return result;
    }
}