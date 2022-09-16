using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TutorialContent
{
    public Sprite sprite; //튜토리얼 이미지
    [TextArea(0, 5)]
    public string explain; //설명 text 내용
}

[System.Serializable]
public struct TutorialRef
{
    public GameObject camera;
    public Image image;
    public Text text;
    [Tooltip("0: 확인(닫기), 1: 다음, 2: 이전")]
    public GameObject button;
    public GameObject mainCanvas;    // 카메라에 찍힐 내용들의 부모 오브젝트
}

public class PuzzleTutorial : MonoBehaviour
{
    // 한 페이지에 3개의 정보를 보여줌
    private const int PENEL_INFO_CNT = 3;

    [Header("참조")]
    public GameObject tutorialUICanvas;
    public TutorialRef[] tutorialPages = new TutorialRef[PENEL_INFO_CNT];

    [Header("튜토리얼 정보")]
    // 인스펙터에 작성
    public TutorialContent[] tutorialinfo;

    // 실제 튜토리얼에 보일 내용
    private GameObject[] firstContent;
    private GameObject[] secondContent;
    private GameObject[] thirdContent;

    // 현재 공개되어야 하는 정보의 최대 개수
    private int maxInfoSize;
    private int GetMaxPageIdx() => (maxInfoSize - 1) / PENEL_INFO_CNT;

    // 현재 가리키고 있는 페이지
    private int curPageIdx;


    // 실제로 다른 클래스에서 쓰는 함수
    public void SetTutorial(int _maxInfoSize, int _showPage, bool isShow = true)
    {
        maxInfoSize = _maxInfoSize;
        ShowContentCanvas(isShow);
        ShowPage(_showPage - 1);
    }

    private void Awake()
    {
        GameObject[] content;
        int size, i, j;

        for (i = 0; i < PENEL_INFO_CNT; ++i)
        {
            size = tutorialPages[i].mainCanvas.transform.childCount;
            switch (i)
            {
                case 0: firstContent = new GameObject[size]; content = firstContent;  break;
                case 1: secondContent = new GameObject[size]; content = secondContent; break;
                default: thirdContent = new GameObject[size]; content = thirdContent; break;
            }
            j = -1;
            foreach (Transform item in tutorialPages[i].mainCanvas.transform)
            {
                content[++j] = item.gameObject;
            }
        }
    }

    private GameObject[] GetContent(int idx)
    {
        switch (idx)
        {
            case 0: return firstContent;
            case 1: return secondContent;
            case 2: return thirdContent;
        }
        return null;
    }

    private void ShowContentCanvas(bool flag)
    {
        for (int i = 0; i < PENEL_INFO_CNT; ++i)
        {
            tutorialPages[i].mainCanvas.SetActive(flag);
        }
    }

    private void SetButton(int infoIdx, int pageIdx)
    {
        foreach (var item in tutorialPages)
        {
            item.button.SetActive(false);
        }

        int CLOSE = 0;
        int NEXT = 1;
        int PREV = 2;

        // 첫 페이지
        if (infoIdx == 0)
        {
            // 첫 페이지가 마지막 페이지
            if (GetMaxPageIdx() == pageIdx)
            {
                tutorialPages[CLOSE].button.SetActive(true);
            }
            else
            {
                tutorialPages[NEXT].button.SetActive(true);
            }
        }

        // 마지막 페이지
        else if (GetMaxPageIdx() == pageIdx)
        {
            tutorialPages[CLOSE].button.SetActive(true);
            tutorialPages[PREV].button.SetActive(true);
        }

        //중간 페이지
        else
        {
            tutorialPages[NEXT].button.SetActive(true);
            tutorialPages[PREV].button.SetActive(true);
        }
    }

    private void SetContents(int infoIdx, int pageIdx)
    {
        GameObject[] content;
        for (int i = 0; i < PENEL_INFO_CNT; ++i)
        {
            if (infoIdx + i >= maxInfoSize) break;
            content = GetContent(i);
            content[pageIdx].SetActive(true);
        }
    }

    // idx의 정보를 보여줌
    private void ShowInfo(int infoIdx, int panlInfoIdx)
    {
        // 만약 제한 정보를 넘기거나 설명이 없는 칸은 빈 칸으로 남긴다.
        if (infoIdx >= maxInfoSize || tutorialinfo[infoIdx].explain == string.Empty)
        {
            tutorialPages[panlInfoIdx].camera.SetActive(false);
            tutorialPages[panlInfoIdx].image.gameObject.SetActive(false);
            tutorialPages[panlInfoIdx].text.text = string.Empty;
            return;
        }

            // 이미지 사용
            if (tutorialinfo[infoIdx].sprite != null)
        {
            tutorialPages[panlInfoIdx].camera.SetActive(false);
            tutorialPages[panlInfoIdx].image.gameObject.SetActive(true);
            tutorialPages[panlInfoIdx].image.sprite = tutorialinfo[infoIdx].sprite;
        }
        // 카메라 사용
        else
        {
            tutorialPages[panlInfoIdx].camera.SetActive(true);
        }

        // 설명 텍스트
        tutorialPages[panlInfoIdx].text.text = tutorialinfo[infoIdx].explain;
    }

    // 어떤 page를 보여줄 지 보여줌
    private void ShowPage(int pageIdx)
    {
        tutorialUICanvas.SetActive(true);

        int infoIdx = pageIdx * 3;
        SetContents(infoIdx, pageIdx);

        for (int i = 0; i < PENEL_INFO_CNT; ++i)
        {
            ShowInfo(infoIdx + i, i);
        }

        curPageIdx = pageIdx;
        SetButton(infoIdx, pageIdx);
    }



    private void TurnOffCameraObject(int pageIdx)
    {
        GameObject[] content;
        int idx = pageIdx * 3;
        for (int i = 0; i < PENEL_INFO_CNT; ++i)
        {
            if (idx + i >= maxInfoSize) break;
            content = GetContent(i);
            if (content.Length == 0) break;
            content[pageIdx].SetActive(false);
        }
    }

    public void NextButton()
    {
        TurnOffCameraObject(curPageIdx);
        ++curPageIdx;
        ShowPage(curPageIdx);
    }

    public void PrevButton()
    {
        TurnOffCameraObject(curPageIdx);
        --curPageIdx;
        ShowPage(curPageIdx);
    }

    public void ComfirmButton()
    {
        for (int i = 0; i < 3; i++)
        {
            tutorialPages[i].camera.SetActive(false);
        }

        TurnOffCameraObject(GetMaxPageIdx());
        tutorialUICanvas.SetActive(false);
        ShowContentCanvas(false);
    }
}
