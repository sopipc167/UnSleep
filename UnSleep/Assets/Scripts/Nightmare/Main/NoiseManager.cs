using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoiseManager : MonoBehaviour
{
    static public NoiseManager instance;
    public TextManager TM;
    public string con;

    public DragManager DM;
    public Image backGround;

    public NightMareManager NM;
    public bool isStart;

    public FadeInOut fadeinout;
    public Image Fade;
    public BlinkAnimation BA;

    void Start()
    {
        instance = this;
        Dialogue_Proceeder.instance.UpdateCurrentEpiID(6);
        Dialogue_Proceeder.instance.UpdateCurrentDiaID(2001);
        TM.DiaUI.SetActive(true);
        TM.Increasediaindex = true;
    }


    void Update()
    {
        if(con == "BackGround_On")
        {
            backGround.enabled = true;
        }
        else if(con == "BackGround_Off")
        {
            backGround.enabled = false;
        }
        else if(con == "Blink")
        {
            Color tmp = Fade.color;
            tmp.a = 0;
            Fade.color = tmp;
            BA.BlinkOpen();
            con = null;
        }
    }

    public void coStart(int cur, int next)
    {
        StartCoroutine(GameStart(cur, next));
    }

    IEnumerator GameStart(int currentScene, int nextScene)
    {
        isStart = true;
        NM.Case = currentScene;
        fadeinout.Blackout_Func(0.5f);
        yield return new WaitForSeconds(0.3f);
        NM.Case = nextScene;
        con = null;
        isStart = false;
    }

    public IEnumerator GameClear()
    {
        DM.GameSetting(false);
        yield return new WaitForSeconds(0.5f);
        TM.DiaUI.SetActive(true);
        TM.Increasediaindex = true;
    }
}
