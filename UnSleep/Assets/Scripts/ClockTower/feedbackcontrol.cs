using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class feedbackcontrol : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] Levels;
    public GameObject ClearUI;
    private GameObject LevelChecker;

    private void Start()
    {
        LevelChecker = GameObject.Find("LevelChecker");
    }

    public void TurnOffAll()
    {
        LevelChecker = GameObject.Find("LevelChecker");
        LevelChecker.GetComponent<Clear_Condition>().ClearCount = 0f;

        for (int i=0; i < Levels.Length; i++)
        {
            if (Levels[i].activeSelf)
            {
                Levels[i].SetActive(false);
            }
        }
        ClearUI.SetActive(false);
    }


    public void Turn0()
    {

        Levels[0].SetActive(true);
        LevelChecker = GameObject.Find("LevelChecker");
        LevelChecker.GetComponent<Clear_Condition>().ClearCount = 0f;
    }

    public void Turn1()
    {
        Levels[1].SetActive(true);
        LevelChecker = GameObject.Find("LevelChecker");
        LevelChecker.GetComponent<Clear_Condition>().ClearCount = 0f;

    }

    public void Turn2()
    {
        Levels[2].SetActive(true);
        LevelChecker = GameObject.Find("LevelChecker");
        LevelChecker.GetComponent<Clear_Condition>().ClearCount = 0f;

    }

    public void Turn3()
    {
        Levels[3].SetActive(true);
        LevelChecker = GameObject.Find("LevelChecker");
        LevelChecker.GetComponent<Clear_Condition>().ClearCount = 0f;

    }

    public void Turn4()
    {
        Levels[4].SetActive(true);
        LevelChecker = GameObject.Find("LevelChecker");
        LevelChecker.GetComponent<Clear_Condition>().ClearCount = 0f;

    }

    public void Turn5()
    {
        Levels[5].SetActive(true);
        LevelChecker = GameObject.Find("LevelChecker");
        LevelChecker.GetComponent<Clear_Condition>().ClearCount = 0f;

    }

    public void Turn6()
    {
        Levels[6].SetActive(true);
        LevelChecker = GameObject.Find("LevelChecker");
        LevelChecker.GetComponent<Clear_Condition>().ClearCount = 0f;

    }

    public void Turn7()
    {
        Levels[7].SetActive(true);
        LevelChecker = GameObject.Find("LevelChecker");
        LevelChecker.GetComponent<Clear_Condition>().ClearCount = 0f;

    }

    public void Turn8()
    {
        Levels[8].SetActive(true);
        LevelChecker = GameObject.Find("LevelChecker");
        LevelChecker.GetComponent<Clear_Condition>().ClearCount = 0f;

    }

    public void Turn9()
    {
        Levels[9].SetActive(true);
        LevelChecker = GameObject.Find("LevelChecker");
        LevelChecker.GetComponent<Clear_Condition>().ClearCount = 0f;

    }

    public void Turn10()
    {
        Levels[10].SetActive(true);
        LevelChecker = GameObject.Find("LevelChecker");
        LevelChecker.GetComponent<Clear_Condition>().ClearCount = 0f;

    }

 

    public void QuitF()
    {
        Application.Quit();
    }

}
