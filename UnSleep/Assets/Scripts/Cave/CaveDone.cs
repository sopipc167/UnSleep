using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveDone : MonoBehaviour
{
    private GameObject DiaUI;
    private PuzzleClear puzzleClear;
    void Start()
    {
        DiaUI = GameObject.Find("Canvas").transform.GetChild(3).GetChild(1).GetChild(3).gameObject;
        puzzleClear = GameObject.Find("Clear").transform.GetChild(0).GetComponent<PuzzleClear>();
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

        if (CurEpiId == 19)
        {
            //CurDiaId--; //잘 있어요만 전환 후 대사가 없음
            //Dialogue_Proceeder.instance.AddCompleteCondition(8024);
            //Dialogue_Proceeder.instance.CurrentDiaID--;
        }
            

        if (CurEpiId == 7)
        {
            if (CurDiaId == 2013)
                puzzleClear.ClearPuzzle(SceneType.Mental, 1f);
            else if (CurDiaId == 2017)
                puzzleClear.ClearPuzzle(SceneType.Dialogue, 1f);
        }
        else if (CurEpiId == 9 || CurEpiId == 11 || CurEpiId == 15 || CurEpiId == 16 || CurEpiId == 18 || CurEpiId == 19) //나중엔 퍼즐 연출로
            puzzleClear.ClearPuzzle(SceneType.Mental, 1f);
        else if (CurEpiId == 2 || CurEpiId == 5 || CurEpiId == 17)
            puzzleClear.ClearPuzzle(SceneType.Dialogue, 1f);
    }



}
