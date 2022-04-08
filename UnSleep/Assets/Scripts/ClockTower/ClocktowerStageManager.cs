using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClocktowerStageManager : MonoBehaviour
{
    public GameObject[] levels;
    private bool COMPLETE = false;
    private bool COMPLETE2 = false;

    void Start()
    {
        setLevel();
    }

    private void setLevel() //씬 전환 시 레벨 셋팅
    {
        switch (Dialogue_Proceeder.instance.CurrentEpiID)
        {
            case 4: levels[0].SetActive(true); break; //18세 시험
            case 5: levels[1].SetActive(true); break; //19세 나의 미래
            case 8: levels[2].SetActive(true); break; //23세 연애 (2, 3)
            case 9: levels[4].SetActive(true); break; //24세 휴학
            case 12: levels[5].SetActive(true); break; //31세 권태기
            case 13: levels[6].SetActive(true); break; //32세 결혼
            case 14: levels[7].SetActive(true); break; //45세 가족 부양
            case 19: levels[8].SetActive(true); break; //잘 있어요 (8 9 10)
        }
    }

    public void GotoNextBtn() //클리어 후 다음으로 버튼 눌렀을 시
    {
        int CurEpiId = Dialogue_Proceeder.instance.CurrentEpiID;

        if ((CurEpiId != 8 && CurEpiId != 19) ||(COMPLETE && COMPLETE2)) //단일 스테이지 or 모든 스테이지 클리어 했으면
        {
            GotoMentalWorld(); //일단 멘탈월드로
            return;
        }


        if (CurEpiId == 8)
        {
            levels[2].SetActive(false);
            levels[3].SetActive(true);
            COMPLETE = COMPLETE2 = true;
        }
        else if (CurEpiId  == 19)
        {
            if (!COMPLETE && !COMPLETE2)
            {
                levels[8].SetActive(false);
                levels[9].SetActive(true);
                COMPLETE = true;
            }
            else if (COMPLETE && !COMPLETE2)
            {
                levels[9].SetActive(false);
                levels[10].SetActive(true);
                COMPLETE2 = true;

            }
        }


    }

    public void GotoMentalWorld()
    {
        Dialogue_Proceeder.instance.UpdateCurrentDiaIDPlus1();
        SceneManager.LoadScene("Mental_World_Map");

    }
}
