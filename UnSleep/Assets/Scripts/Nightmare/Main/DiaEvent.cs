using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DiaEvent : MonoBehaviour
{
    public TextManager TM;
    public GameObject[] ob;
    public GameObject[] Dia;
    public GameObject FirstDia;
    public GameObject SecondDia;
    public int diaIndex;
    public int diaGroupIndex;
    public int EventNum;

    public bool isFirst = true;

    public AudioClip[] audioClip;
    public AudioSource audioSource;

    public FadeInOut fadeinout;
    public Image Fade;
    public BlinkAnimation BA;

    public int next_flase;
    public int next_true;

    private Dialogue_Proceeder dp;

    void Start()
    {
        dp = Dialogue_Proceeder.instance;
        EventNum = 100;
        next_flase = 700;
        next_true = 699;
    }


    void Update()
    {
        diaGroupIndex = TM.Dia_Id;

        if ((diaIndex != dp.CurrentDiaIndex || diaGroupIndex != dp.CurrentDiaID) && !isFirst)
        {
            if (EventNum == 0)
                nextLevel();
            else if (EventNum == 1)
                Shadow(false);
            else if (EventNum == 2)
                Sound(100);
            else if (EventNum == 3)
            {
                Color tmp = Fade.color;
                tmp.a = 255;
                Fade.color = tmp;
            }
            else if(EventNum == 4)
                Move(2, new Vector3(7.54f, -0.95f, 0), new Vector3(0, 0, 0));
            else if(EventNum == 5)
            {
                ob[3].SetActive(false);
                if(diaGroupIndex == 728)
                {
                    Dia[35].SetActive(false);
                    Dia[34].SetActive(true);
                    Dia[36].SetActive(true);
                }
            }

            isFirst = true;
            EventNum = 100;
        }

        if (isFirst)
        {
            if (TM.con != "Sound0" || TM.con != "Sound1" || TM.con == "Sound2")
                Setting();

            if (TM.con == "Shadow")
            {

                //Debug.Log("SHADOW");
                EventNum = 1;
                Shadow(true);
            }
            else if (TM.con == "BearUp")
            {
                EventNum = 100;
                Move(1, new Vector3(10.15f, 0.58f, 0), new Vector3(0, 0, 0));
            }
            else if (TM.con == "Sound0" || TM.con == "Sound1" || TM.con == "Sound2")
            {
                EventNum = 2;
                if (TM.con == "Sound0")
                    Sound(0);
                else if (TM.con == "Sound1")
                    Sound(1);
                else if (TM.con == "Sound2")
                    Sound(2);

                Setting();
            }
            else if (TM.con == "Next")
            {
                EventNum = 0;
            }
            else if (TM.con == "Fadein")
            {
                //fadeinout.Blackout_Func(0.5f);
            }
            else if (TM.con == "BlinkOpen")
            {
                Color tmp = Fade.color;
                tmp.a = 0;
                Fade.color = tmp;
                BA.BlinkOpen();
            }
            else if(TM.con == "BlinkClose")
            {
                EventNum = 3;
                Debug.Log("Black");
                BA.BlinkClose();
            }
            else if(TM.con == "ChairMove")
            {
                EventNum = 4;
            }
            else if(TM.con == "BearDis")
            {
                EventNum = 5;
            }
        }
    }

    void Setting()
    {
        isFirst = false;
        diaIndex = dp.CurrentDiaIndex;
    }

    public void nextLevel()
    {
        Dia[diaGroupIndex - next_flase].SetActive(false);
        Dia[diaGroupIndex - next_true].SetActive(true);
    }

    public void Shadow(bool isOn)
    {
        if (isOn)
        {
            ob[0].SetActive(true);
        }
        else
        {
            Debug.Log("OFF");
            ob[0].SetActive(false);
            FirstDia.SetActive(false);
            SecondDia.SetActive(true);
        }
    }

    public void Move(int index, Vector3 pos, Vector3 angle)
    {
        ob[index].transform.localPosition = pos;
        ob[index].transform.eulerAngles = angle;
    }

    public void Sound(int soundNum)
    {
        if(soundNum < 2)
            audioSource.clip = audioClip[soundNum];

        switch (soundNum)
        {
            case 0:
                Debug.Log("Sound0");
                audioSource.panStereo = 1;
                audioSource.volume = 0.3f;
                audioSource.Play();
                return;
            case 1:
                Debug.Log("Sound1");
                audioSource.panStereo = 1;
                audioSource.volume = 0.7f;
                audioSource.Play();
                return;
            case 2:
                Debug.Log("Sound2");
                audioSource.panStereo = 0.7f;
                audioSource.volume = 0.7f;
                audioSource.Play();
                return;
            default:
                Debug.Log("Stop");
                audioSource.Stop();
                return;
        }
    }
}
