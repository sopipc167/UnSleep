using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaEvent : MonoBehaviour
{
    public TextManager TM;
    public GameObject[] ob;
    public GameObject FirstDia;
    public GameObject SecondDia;
    public int diaIndex;
    public int diaGroupIndex;
    public int EventNum;

    public bool isFirst = true;

    public AudioClip[] audioClip;
    public AudioSource audioSource;

    void Start()
    {
        
    }


    void Update()
    {
        if ((diaIndex != TM.dialogues_index || diaGroupIndex != TM.Dia_index) && !isFirst)
        {
            switch (EventNum)
            {
                case 1:
                    Shadow(false);
                    isFirst = true;
                    break;
                case 2:
                    Sound(100);
                    isFirst = true;
                    break;
                default:
                    isFirst = true;
                    break;
            }
        }

        if (isFirst)
        {
            if (TM.con == "Shadow")
            {
                //Debug.Log("SHADOW");
                Setting();
                EventNum = 1;
                Shadow(true);
            }
            else if (TM.con == "BearUp")
            {
                Setting();
                EventNum = 100;
                BearUp();
            }
            else if(TM.con == "Sound0" || TM.con == "Sound1")
            {
                EventNum = 2;
                if (TM.con == "Sound0")
                    Sound(0);
                else if (TM.con == "Sound1")
                    Sound(1);
                Setting();
            }
        }
    }

    void Setting()
    {
        isFirst = false;
        TM.con = "NULL";
        diaGroupIndex = TM.Dia_index;
        diaIndex = TM.dialogues_index;
    }

    public void Shadow(bool isOn)
    {
        if (isOn)
        {
            //Debug.Log("ON");
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

    public void BearUp()
    {
        ob[1].transform.localPosition = new Vector3(9.23f, 1, 0);
        ob[1].transform.eulerAngles = new Vector3(0, 0, 0);
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
            default:
                Debug.Log("Stop");
                audioSource.Stop();
                return;
        }
    }
}
