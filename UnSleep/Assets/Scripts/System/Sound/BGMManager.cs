using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum BGMType
{
    None, 
}

[System.Serializable]
public class BGMNode
{
    public BGMType type;
    public AudioClip clip;
}

[RequireComponent(typeof(AudioSource))]
public class BGMManager : MonoBehaviour
{
    #region 싱글톤 클래스
    private static BGMManager instance;

    public static BGMManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<BGMManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    BGMManager newObj = Resources.Load<BGMManager>("Singleton/BGM Manager");
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

    private AudioSource source;

    public List<BGMNode> BGMList;
    private readonly Dictionary<BGMType, AudioClip> bgmDic = new Dictionary<BGMType, AudioClip>();

    private float baseVolume = 1f;
    private bool isChanging = false;

    public void PlayBGM(BGMType type)
    {
        if (type == BGMType.None) return;

        AudioClip currentClip = bgmDic[type];

        if (source.isPlaying)
        {
            source.Stop();
        }

        source.clip = currentClip;

        source.Play();
    }

    public void ChangeBGM(BGMType type, float outDelay = 5f, float inDelay = 5f)
    {
        StartCoroutine(ChangeBGMCoroutine(type, outDelay, inDelay));
    }

    public void StopBGM()
    {
        if (source.isPlaying)
        {
            source.Stop();
        }
    }

    public void RestartBGM()
    {
        if (!source.isPlaying)
        {
            source.Play();
        }
    }

    public void FadeOutBGM(float delay = 5f)
    {
        StartCoroutine(FadeOutBGMCoroutine(delay));
    }

    public void FadeInBGM(BGMType type, float delay = 5f)
    {
        PlayBGM(type);
        StartCoroutine(FadeInBGMCoroutine(delay));
    }

    public void SetSEVolume(float volume)
    {
        baseVolume = volume;
        if (!isChanging && source.isPlaying)
        {
            source.volume = volume;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.loop = true;
        source.playOnAwake = true;
        SetDic();
    }

    private void SetDic()
    {
        int size = BGMList.Count;
        for (int i = 0; i < size; ++i)
        {
            bgmDic[BGMList[i].type] = BGMList[i].clip;
        }
    }

    private IEnumerator FadeOutBGMCoroutine(float delay)
    {
        isChanging = true;

        float tmp = baseVolume / delay;
        while (source.volume > 0.001f)
        {
            source.volume -= Time.deltaTime * tmp;
            yield return null;
        }
        source.volume = 0f;

        isChanging = false;
    }

    private IEnumerator FadeInBGMCoroutine(float delay)
    {
        isChanging = true;

        float tmp = baseVolume / delay;
        source.volume = 0f;
        while (source.volume < baseVolume)
        {
            source.volume += Time.deltaTime * tmp;
            yield return null;
        }
        source.volume = baseVolume;

        isChanging = false;
    }

    private IEnumerator ChangeBGMCoroutine(BGMType type, float outDelay, float inDelay)
    {
        FadeOutBGM(outDelay);
        yield return new WaitUntil(() => !isChanging);

        FadeInBGM(type, inDelay);
    }
}
