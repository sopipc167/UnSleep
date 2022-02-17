using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TutorialPage
{
    public bool[] isCamera; //true면 카메라 표시
    public Sprite[] sprites; //튜토리얼 이미지
    public string[] explain; //설명 text 내용
}

public class PuzzleTutorial : MonoBehaviour
{
    public GameObject Tutorial_Panel;
    public GameObject[] cameras;
    public Image[] images;
    public Text[] texts;
    public GameObject[] buttons; //0: 확인(닫기) 1: 다음 2:이전

    //인스펙터에 작성
    public TutorialPage[] tutorialPages;

    //start부터 until까지의 페이지를 보여줌. 
    private int start;
    private int end;
    private int curr;
   

    //(start, end]의 튜토리얼 출력 범위를 정합니다. 
    //레벨에 따라 출력되게끔 씬 시작 시 세팅해주세요
    //초기 출력 후 게임을 진행하고 있을 때'조작법'을 누르면 start를 0으로 셋팅하면 됨
    public void SetOnTutorial(int _start, int _end) 
    {
        start = _start;
        end = _end;
        curr = start;
        SetPuzzleTutorial();
        Tutorial_Panel.SetActive(true);
    }

    public void SetPuzzleTutorial() 
    {
        if (end - start == 1) //한 페이지만 띄워진 경우
        {
            buttons[0].SetActive(true);
            buttons[1].SetActive(false);
            buttons[2].SetActive(false);
            SetPage(curr);
        }
        else if (curr == start) //여러 페이지 중 첫 페이지인 경우
        {
            buttons[0].SetActive(false);
            buttons[1].SetActive(true);
            buttons[2].SetActive(false);
            SetPage(curr);
        }
        else if (curr == end - 1) //여러 페이지 중 마지막 페이지인 경우
        {
            buttons[0].SetActive(true);
            buttons[1].SetActive(false);
            buttons[2].SetActive(true);
            SetPage(curr);
        }
        else
        {
            buttons[0].SetActive(false);
            buttons[1].SetActive(true);
            buttons[2].SetActive(true);
            SetPage(curr);
        }

    }

    
    private void SetPage(int idx) //idx 페이지로 내용을 바꾼다
    {
        for (int i = 0; i < 3; i++)
        {
            //카메라 사용 or 이미지 출력
            if (tutorialPages[idx].isCamera[i])
                cameras[i].SetActive(true);
            else
            {
                cameras[i].SetActive(false);
                images[i].sprite = tutorialPages[idx].sprites[i];

            }

            //설명 텍스트
            texts[i].text = tutorialPages[idx].explain[i];
        }

    }

    public void NextButton()
    {
        curr++;
        if (curr >= tutorialPages.Length) //그럴일은 없겠지만 넘치는 경우 방지
            curr = tutorialPages.Length - 1;
        SetPuzzleTutorial();
    }

    public void PrevButton()
    {
        curr--;
        if (curr < 0) //그럴일은 없겠지만 넘치는 경우 방지
            curr = 0;
        SetPuzzleTutorial();
    }

    public void ComfirmButton()
    {
        for (int i = 0; i < 3; i++)
            cameras[i].SetActive(false);

        Tutorial_Panel.SetActive(false);
    }

}
