using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum SEType
{
    None, 
}

[System.Serializable]
public class SENode
{
    public SEType type;
    public AudioClip clip;
}

public class SEManager : MonoBehaviour
{
    #region 싱글톤 클래스
    private static SEManager instance;

    public static SEManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<SEManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    SEManager newObj = Resources.Load<SEManager>("Singleton/SE Manager");
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

    public int audioSourceCount;
    public List<SENode> seList;
    private readonly Dictionary<SEType, AudioClip> seDic = new Dictionary<SEType, AudioClip>();

    private AudioSource[] sources;
    private int curSource = -1;

    public void PlaySE(SEType type, float stero = 0f, float pitch = 1f)
    {
        if (type == SEType.None) return;

        ++curSource;
        if (curSource == audioSourceCount)
        {
            curSource = 0;
        }

        AudioClip currentClip = seDic[type];
        AudioSource source = sources[curSource];

        if (source.isPlaying)
        {
            source.Stop();
        }

        source.clip = currentClip;
        source.panStereo = stero;
        source.pitch = pitch;

        source.Play();
    }

    public void StopAllSE()
    {
        for (int i = 0; i < audioSourceCount; ++i)
        {
            sources[i].Stop();
        }
    }

    public void SetSEVolume(float volume)
    {
        for (int i = 0; i < audioSourceCount; ++i)
        {
            sources[i].volume = volume;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        sources = new AudioSource[audioSourceCount];
        for (int i = 0; i < audioSourceCount; ++i)
        {
            GameObject sourceObj = new GameObject("SE Source" + (i + 1), typeof(AudioSource));
            sourceObj.transform.SetParent(transform);
            sources[i] = sourceObj.GetComponent<AudioSource>();
            sources[i].loop = false;
            sources[i].playOnAwake = false;
        }
        SetDic();
    }

    private void SetDic()
    {
        int size = seList.Count;
        for (int i = 0; i < size; ++i)
        {
            seDic[seList[i].type] = seList[i].clip;
        }
    }
}
