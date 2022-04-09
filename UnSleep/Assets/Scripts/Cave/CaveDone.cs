using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveDone : MonoBehaviour
{
    private GameObject DiaUI;
    void Start()
    {
        DiaUI = GameObject.Find("DiaUI");
    }

    // Update is called once per frame
    void Update()
    {
        if (!DiaUI.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GotoNextc();
            }
        }
    }

    public void GotoNextc()
    {
        int CurEpiId = Dialogue_Proceeder.instance.CurrentEpiID;
        int CurDiaId = Dialogue_Proceeder.instance.CurrentDiaID;

        if (CurEpiId == 7)
        {
            if (CurDiaId == 2013)
                GoMWM();
            else if (CurDiaId == 2017)
                GoDT();
        }
        else if (CurEpiId == 9 || CurEpiId == 11 || CurEpiId == 15 || CurEpiId == 16) //나중엔 퍼즐 연출로
            GoMWM();
        else if (CurEpiId == 17)
            GoDT();
    }

    private void GoMWM()
    {
        Dialogue_Proceeder.instance.UpdateCurrentDiaIDPlus1();
        SceneManager.LoadScene("Mental_World_Map");

    }
    private void GoDT()
    {
        Dialogue_Proceeder.instance.UpdateCurrentDiaIDPlus1();
        SceneManager.LoadScene("DialogueTest");

    }

}
