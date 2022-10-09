using System.Collections;
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
    public AudioSource bgmSource;
    public AudioSource seSource1;
    public AudioSource seSource2;
    private AudioSource seSource;

    private int seState = 0;

    private IEnumerator bgmCoroutine;

    private bool[] isMute = { false, false, false };

    // bgm
    private bool isChanging = false;

    // se
    private readonly Dictionary<string, AudioClip> seDic = new Dictionary<string, AudioClip>();


    public void Clear()
    {
        seDic.Clear();
    }

    // BGM
    #region BGM

    public void PlayBGM(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        
        StopAllCoroutines();
        bgmSource.volume = volume;

        if (bgmSource.isPlaying) bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void PlayBGM(string name, float volume = 1f)
    {
        AudioClip currentClip = GetAudioClip(name, SoundType.BGM);
        if (currentClip == null) return;

        StopAllCoroutines();
        bgmSource.volume = volume;

        if (bgmSource.isPlaying) bgmSource.Stop();
        bgmSource.clip = currentClip;
        bgmSource.Play();
    }

    public void ChangeBGM(AudioClip clip, float volume = 1f, float outDelay = 5f, float inDelay = 5f)
    {
        if (bgmCoroutine != null) StopCoroutine(bgmCoroutine);
        bgmCoroutine = ChangeBGMCoroutine(clip, volume, outDelay, inDelay);
        StartCoroutine(bgmCoroutine);
    }

    public void ChangeBGM(string name, float volume = 1f, float outDelay = 5f, float inDelay = 5f)
    {
        if (bgmCoroutine != null) StopCoroutine(bgmCoroutine);
        bgmCoroutine = ChangeBGMCoroutine(name, volume, outDelay, inDelay);
        StartCoroutine(bgmCoroutine);
    }

    public void PauseBGM()
    {
        if (bgmSource.isPlaying) bgmSource.Pause();
    }

    public void UnPauseBGM()
    {
        if (!bgmSource.isPlaying) bgmSource.UnPause();
    }

    public void FadeOutBGM(float volume = 0.001f, float delay = 5f)
    {
        if (bgmCoroutine != null) StopCoroutine(bgmCoroutine);
        bgmCoroutine = FadeOutBGMCoroutine(delay, volume);
        StartCoroutine(bgmCoroutine);
    }

    public void FadeInBGM(AudioClip clip, float volume = 1f, float delay = 5f)
    {
        if (bgmSource.clip == clip) return;
        if (bgmCoroutine != null) StopCoroutine(bgmCoroutine);
        PlayBGM(clip, 0f);
        bgmCoroutine = FadeInBGMCoroutine(delay, volume);
        StartCoroutine(bgmCoroutine);
    }

    public void FadeInBGM(string name, float volume = 1f, float delay = 5f)
    {
        if (bgmCoroutine != null) StopCoroutine(bgmCoroutine);
        PlayBGM(name, 0f);
        bgmCoroutine = FadeInBGMCoroutine(delay, volume);
        StartCoroutine(bgmCoroutine);
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
        CheckAndPlaySE(currentClip, volume);
    }

    public void PlaySE(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        CheckAndPlaySE(clip, volume);
    }


    public void StopSE()
    {
        StartCoroutine(FadeOutSECoroutine(0.5f));
    }
    #endregion


    //볼륨 조절 함수
    #region 볼륨 조절

    public void SetVolume(SoundType type, float volume)
    {
        if (volume < 0.001f) volume = 0.00000001f;
        mainMixer.SetFloat(type.ToString(), Mathf.Log10(volume) * 20);
    }

    public void SetMute(SoundType type, bool isOn)
    {
        isMute[(int)type] = isOn;

        if (type != SoundType.SE)
        {
            if (isMute[(int)SoundType.Master] || isMute[(int)SoundType.BGM])
            {
                bgmSource.mute = true;
                bgmSource.Stop();
            }
            else
            {
                bgmSource.mute = false;
                bgmSource.Play();
            }
        }
        if (type != SoundType.BGM)
        {
            if (isMute[(int)SoundType.Master] || isMute[(int)SoundType.SE])
            {
                seSource1.mute = true;
                seSource2.mute = true;
            }
            else
            {
                seSource1.mute = false;
                seSource2.mute = false;
            }
        }
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

        bgmCoroutine = null;
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

        bgmCoroutine = null;
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

    // 10초 이상인 SE가 2개일 리는 없다는 가정임.
    private void CheckAndPlaySE(AudioClip clip, float volume)
    {
        seSource = seSource1;
        if (seState == 1) seSource = seSource2;

        // 7초 이상인 SE라면 현재 상태에 check
        if (clip.length > 10f)
        {
            if (seSource == seSource1) seState = 1;
            else seState = 2;
        }

        seSource.PlayOneShot(clip, volume);
    }

    private IEnumerator FadeOutSECoroutine(float delay, float volume = 0.001f)
    {
        AudioSource curSource = seSource1;
        if (seState == 2) curSource = seSource2;

        float tmp = 1f / delay;
        while (curSource.volume > volume)
        {
            curSource.volume -= Time.deltaTime * tmp;
            yield return null;
        }
        curSource.Stop();
        curSource.volume = 1f;
        if (curSource == seSource1) seState = 1;
        else seState = 2;
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