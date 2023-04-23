using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockLevelManager : MonoBehaviour
{
    public PuzzleClear puzzleClear;
    public Image blinkPanel;
    public GameObject clearUI;
    public GameObject[] levels;
    public Text mission;

    private BCogWheel[] bCogWheels;
    private float clearCount = 0f;
    private bool tiktoking = false;
    private Coroutine coroutine;

    private int currentStageIndex = 0;

    void Start()
    {
        setLevel();
        SoundManager.Instance.PlayBGM("scifi");
        bCogWheels = FindObjectsOfType<BCogWheel>();
    }


    private void setLevel() //씬 전환 시 레벨 셋팅
    {
        switch (Dialogue_Proceeder.instance.CurrentEpiID)
        {
            case 4:
                setStageInfo(0, "표시된 회전 방향으로 돌도록 톱니를 연결해보자.");
                break; //18세 시험
            case 5:
                setStageInfo(1, "표시된 회전 방향으로 돌도록 톱니를 연결해보자.");
                break; //19세 
            case 8:
                setStageInfo(3, "톱니가 너무 빠르다! 톱니바퀴의 속도를 줄여보자");
                break; //23세 연애 (2, 3)
            case 9:
                setStageInfo(4, "표시된 회전 방향으로 돌도록 톱니를 연결해보자.");
                break; //24세 휴학
            case 12:
                setStageInfo(5, "톱니의 속도를 올려보자!");
                break; //31세 권태기
            case 13:
                setStageInfo(6, "표시된 조건에 맞게 톱니를 연결해보자.");
                break; //32세 결혼
            case 14:
                setStageInfo(7, "표시된 속도 조건에 맞게 톱니를 연결해보자.");
                break; //45세 가족 부양
            case 19:
                setStageInfo(8, "표시된 조건에 맞게 톱니를 연결해보자.");
                Dialogue_Proceeder.instance.AddCompleteCondition(30);
                break; //잘 있어요 (8 9 10)
        }
    }


    private void checkClear()
    {
        if (bCogWheels.Count(bcw => !bcw.satisfy) == 0)
        {
            clearCount += 1f * Time.deltaTime;
            if (!tiktoking) 
                coroutine = StartCoroutine(TicTok());
        } else
        {
            clearCount = 0f;
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }

        }
    }


    void Update()
    {
        checkClear();

        if (clearCount >= 4f) clear();
    }
    public void clear()
    {
        if (Dialogue_Proceeder.instance.CurrentEpiID == 19 && Dialogue_Proceeder.instance.CurrentDiaID == 8036) //잘있어요-1 클리어 시
        {
            Dialogue_Proceeder.instance.AddCompleteCondition(31);
            clearUI.SetActive(true);
            // textManager.Set_Dialogue_Goodbye();
            clearCount = 0f;
        }
        else if (Dialogue_Proceeder.instance.CurrentEpiID == 19 && Dialogue_Proceeder.instance.CurrentDiaID == 8037)
        {
            Dialogue_Proceeder.instance.AddCompleteCondition(32);
            clearUI.SetActive(true);
            // textManager.Set_Dialogue_Goodbye();
            clearCount = 0f;
        }
        else
        {
            puzzleClear.ClearPuzzle(SceneType.Mental, 0f);
            SoundManager.Instance.FadeOutBGM();
        }
    }



    public void gotoNextBtn() //클리어 후 다음으로 버튼 눌렀을 시
    {
        levels[currentStageIndex].SetActive(false);
        currentStageIndex++;
        levels[currentStageIndex].SetActive(true);
        clearUI.SetActive(false);

        if (currentStageIndex == 10)
        {
            mission.gameObject.SetActive(false);
            Dialogue_Proceeder.instance.AddCompleteCondition(33);
        }
    }

    private void setStageInfo(int idx, string missionText)
    {
        levels[idx].SetActive(true);
        currentStageIndex = idx;
        mission.text = missionText;
    }

    IEnumerator TicTok()
    {

        SoundManager.Instance.PlaySE("tictokshort");
        float time = 0f;
        tiktoking = true;

        Color color = blinkPanel.color;
        color.a = Mathf.Lerp(0f, 0.4f, time);

        while (color.a < 0.4f)
        {
            time += Time.deltaTime / 0.5f;
            color.a = Mathf.Lerp(0f, 0.4f, time);
            blinkPanel.color = color;

            yield return null;

        }
        time = 0f;

        while (color.a > 0f)
        {
            time += Time.deltaTime / 0.5f;
            color.a = Mathf.Lerp(0.4f, 0f, time);
            blinkPanel.color = color;

            yield return null;

        }


        tiktoking = false;
        yield return null;
    }
}
