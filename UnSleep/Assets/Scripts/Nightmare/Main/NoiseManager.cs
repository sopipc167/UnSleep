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
        SoundManager.Instance.PlayBGM("instinct");
        //Dialogue_Proceeder.instance.UpdateCurrentEpiID(6);
        //Dialogue_Proceeder.instance.UpdateCurrentDiaID(2001);
        TM.DiaUI.SetActive(true);
        TM.Increasediaindex = true;
    }


    void Update()
    {
        if(con != null)
        {
            if (con == "BackGround_On")
            {
                Debug.Log("BackGround_On");
                backGround.enabled = true;
            }
            else if (con == "BackGround_Off")
            {
                backGround.enabled = false;
            }
            else if (con == "Blink")
            {
                Color tmp = Fade.color;
                tmp.a = 0;
                Fade.color = tmp;
                BA.BlinkOpen();
            }

            con = null;
        }
        
    }

    public void coStart(int cur, int next)
    {
        StartCoroutine(GameStart(cur, next));
    }

    IEnumerator GameStart(int currentScene, int nextScene)
    {
        SoundManager.Instance.FadeOutBGM(delay: 1.0f);
        isStart = true;
        //현재 씬 종료
        NM.Case = currentScene;
        fadeinout.Blackout_Func(0.5f);
        yield return new WaitForSeconds(0.3f);
        //다음 씬 로드
        NM.Case = nextScene;
        con = null;
        isStart = false;
    }

    public IEnumerator GameClear()
    {
        //현재 게임 상황 종료(?)
        DM.GameSetting(false);
        yield return new WaitForSeconds(0.5f);
        //대사창 실행
        TM.DiaUI.SetActive(true);
        TM.Increasediaindex = true;
    }
}
