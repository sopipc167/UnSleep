using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pages : MonoBehaviour
{
    public Button btn;
    Color tmp;

    public Text txt;
    public float speed = 0.2f;
    public bool isEnd;
    public bool isStart;
    public string message;

    public static int cPage;
    public Book_test book;

    public Auto auto;
    public float DelayTime;
    public float AnimationFrame;
    public float BetweenTime;

    public string[] PageString;
    public Sprite[] PageSprite;
    public Text[] pageText; // 0->후일담1 / 1->캐릭터설명1 / 2->제목1 / 3->요약1 / 4->후일담2 / 5->캐릭터설명2 / 6->제목2 / 7->요약2
    public Image[] pageImage; // 0->캐릭터1 / 1->썸네일1 / 2->캐릭터2 / 3->썸네일2

    public bool isChange;
    int isNext = 1;
    int j = 0;
    int text = 0;
    int image = 0;
    public bool isBack;

    public Cover cover;

    public int epi;

    void Start()
    {
        isBack = true;
        tmp = btn.GetComponent<Image>().color;
        message = "피곤한 상태로 잠에 들었더니 잠을 푹 잘 수 있었다. 잠을 푹  자니 불안한 마음이 조금은 사라졌다. " +
            "아무래도 어제 불안하고  초조했던 것은 잠을 제대로 자지 못해서 그런 것 같다. 오늘은  평소와 다르게 아침밥을 먹고 학교에 갔다. " +
            "한결 놓인 마음으로 시험을 볼 수 있었다. 다행히도 시험에 아는 문제들이 많이 보였다. 이번 수학 시험은 전보다 점수가 올랐다. 얼마나  기뻤는지 모른다.";
    }
    //message 데이터 전달 방식 정리 필요


    void Update()
    {
        if (Dialogue_Proceeder.instance != null)
        {
            if (string.Equals(Dialogue_Proceeder.instance.End, "E") && !isStart)
            {
                Debug.Log("isEnd = true");
                auto.Mode = FlipMode.RightToLeft;
                cover.isBack = true;
                Invoke("Flipping", 1);
                isStart = true;
                Invoke("StartTyping", 1);
            }
        }

        if (isEnd)
        {
            StartCoroutine(Typing(txt, message, speed));
            isEnd = false;
        }

        if (!isChange && book.bookPages.Length - 2 > book.currentPage)
        {
            Debug.Log("Change");
            if (isNext < 0)
                j = 4;
            else
                j = 0;

            //error
            for (int i = j; i < j + 4; i++)
            {
                pageText[i].text = PageString[text];
                text++;
            }

            if (isNext < 0)
                j = 2;
            else
                j = 0;

            for (int i = j; i < j + 2; i++)
            {
                pageImage[i].sprite = PageSprite[image];
                image++;
            }

            isNext *= -1;
            isChange = true;
            Debug.Log(isChange);
        }
        else
        {
            isChange = true;
        }

        if (!isBack && book.currentPage > 2 && book.currentPage <= book.bookPages.Length - 4)
        {
            Debug.Log("Back");
            isNext *= -1;
            text -= 12;
            image -= 6;

            if (isNext < 0)
                j = 4;
            else
                j = 0;

            for (int i = j; i < j + 4; i++)
            {
                pageText[i].text = PageString[text];
                text++;
            }

            if (isNext < 0)
                j = 2;
            else
                j = 0;

            for (int i = j; i < j + 2; i++)
            {
                pageImage[i].sprite = PageSprite[image];
                image++;
            }

            isNext *= -1;
            isBack = true;
        }
        else
        {
            isBack = true;
        }
    }

    public void Flipping()
    {
        StartCoroutine(auto.FlipToCurrentPage(DelayTime, AnimationFrame, BetweenTime, cPage));
    }

    public void StartTyping()
    {
        isEnd = true;
    }

    public void Enter()
    {
        btn.GetComponent<Image>().color = Color.white;
    }

    public void Exit()
    {
        btn.GetComponent<Image>().color = tmp;
    }

    public void Click()
    {
        cPage = book.currentPage;
        Debug.Log(epi);
        if (Dialogue_Proceeder.instance != null)
        {
            Dialogue_Proceeder.instance.CurrentEpiID = epi;
            Debug.Log(Dialogue_Proceeder.instance.CurrentDiaID);
        }
        SceneManager.LoadScene("DialogueTest");
    }

    IEnumerator Typing(Text typingText, string message, float speed)
    {
        for(int i = 0; i < message.Length; i++)
        {
            typingText.text = message.Substring(0, i + 1);
            yield return new WaitForSeconds(speed);
        }
    }
}
