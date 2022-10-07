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
        else if(con == "GameStart_1")
        {
            DM.GameSetting(true);
            con = null;
        }
        else if(con == "GameStart_2")
        {
            StartCoroutine(GameStart(2, 3));
        }
    }

    IEnumerator GameStart(int lastScene, int currentScene)
    {
        NM.Case = lastScene;
        //페이드인아웃
        yield return new WaitForSeconds(0.5f);
        NM.Case = currentScene;
    }

    public IEnumerator GameClear()
    {
        DM.GameSetting(false);
        yield return new WaitForSeconds(0.5f);
        TM.DiaUI.SetActive(true);
        TM.Increasediaindex = true;
    }
}
