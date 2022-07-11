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
    public Text[] pageText; 
    // 0->후일담1 / 1->캐릭터설명1 / 2->제목1 / 3->요약1 / 4->후일담2 / 5->캐릭터설명2 / 6->제목2 / 7->요약2
    public Image[] pageImage;
    // 0->캐릭터1 / 1->썸네일1 / 2->캐릭터2 / 3->썸네일2

    public DiaryTextParser diaryTextParser; //일기장 텍스트 파싱하는 애
    public List<DiaryText> diaryText; //파싱된 데이터가 담김
    

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
        diaryText = diaryTextParser.ParseDiaryText(); // 일기장 텍스트 데이터 파싱

        isBack = true;
        tmp = btn.GetComponent<Image>().color;
        message = "피곤한 상태로 잠에 들었더니 잠을 푹 잘 수 있었다. 잠을 푹  자니 불안한 마음이 조금은 사라졌다. " +
            "아무래도 어제 불안하고  초조했던 것은 잠을 제대로 자지 못해서 그런 것 같다. 오늘은  평소와 다르게 아침밥을 먹고 학교에 갔다. " +
            "한결 놓인 마음으로 시험을 볼 수 있었다. 다행히도 시험에 아는 문제들이 많이 보였다. 이번 수학 시험은 전보다 점수가 올랐다. 얼마나  기뻤는지 모른다.";
    }


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
            StartCoroutine(Typing(txt, message, speed)); //후일담 타이핑 효과
            isEnd = false;
        }

        if (!isChange && book.bookPages.Length - 2 > book.currentPage) //오른쪽으로 넘길 경우(페이지 업데이트)
        {
            Debug.Log("isChange");
            Debug.Log("text: " + text + "image: " + image + "isNext: " + isNext);

            //ChangePage();
            Re_ChangePage();
            isChange = true; 
            //Book_test.cs 361줄에서(오른쪽으로 넘기려고 클릭하면) isChange -> false
        }
        else
        {
            isChange = true;
        }

        if (!isBack && book.currentPage <= book.bookPages.Length - 2) //왼쪽으로 넘길 경우(페이지 전으로 돌리기)
        {
            Debug.Log("isBack " + text);

            //페이지를 전과 같이 돌려놓기 위해서
            //isNext *= -1;
            text -= 8;
            image -= 4;

            //ChangePage();
            Re_ChangePage();

            isBack = true;
        }
        else
        {
            isBack = true;
        }
    }

    public void ChangePage()
    {
        //page1-2 or page3-4 둘 중 어떤 묶음을 바꿀 것인지 결정
        //현재 페이지 말고 다음 페이지 기준
        if (isNext < 0) //isNext == -1 -> page3-4 / isNext == 1 -> page1-2
            j = 4;
        else
            j = 0;

        for (int i = j; i < j + 4; i++) //text 변경
        {
            pageText[i].text = PageString[text];
            text++;
        }

        //page1-2 or page3-4 둘 중 어떤 묶음을 바꿀 것인지 결정
        if (isNext < 0) //isNext == -1 -> page3-4 / isNext == 1 -> page1-2
            j = 2;
        else
            j = 0;

        for (int i = j; i < j + 2; i++) //image 변경
        {
            pageImage[i].sprite = PageSprite[image];
            image++;
        }

        isNext *= -1; //묶음을 바꾸고 다음 묶음으로 바꾸기
    }

    public void Re_ChangePage()
    {
        // 일기장 내용을 에피소드 별로 관리 -> 현재 보고 있는 페이지가 어디 에피소드인지 알면 쉬워짐
        // Book_test의 CurrentPage/2 를 현재 페이지라고 할 수 있음 (2씩 올라가더라)
        // 홀수 장인 경우 page1-2를 보고 있을 것이고, 짝수 장인 경우 page3-4를 보고 있을 것이다.
        // CurrentPage/2: 홀수 = page3-4 갱신, 짝수 = page1-2 갱신

        int curPage = book.currentPage/2; //현재 보고 있는 페이지
        int curEpi = curPage; // 에피소드 id는 0부터 시작해서 페이지 숫자보다 -1

        if (curPage%2==0)
        {
            pageText[0].text = diaryText[curEpi].afterstory; // 후일담
            pageText[1].text = diaryText[curEpi].characs[0].intro; //캐릭터설명? -> 따로 빼서 수정할 것
            pageText[2].text = diaryText[curEpi].epi_title; // 제목
            pageText[3].text = diaryText[curEpi].epi_intro; // 요약
        }
        else
        {
            pageText[4].text = diaryText[curEpi].afterstory; // 후일담
            pageText[5].text = diaryText[curEpi].characs[0].intro; //캐릭터설명? -> 따로 빼서 수정할 것
            pageText[6].text = diaryText[curEpi].epi_title; // 제목
            pageText[7].text = diaryText[curEpi].epi_intro; // 요약
        }

    }

    public void FlipCheck(bool isRight)
    {

        if (isRight && book.bookPages.Length - 2 > book.currentPage && book.currentPage > 0)
        {
            isNext *= -1;
            text -= 4;
            image -= 2;
        }
        else if (!isRight && book.bookPages.Length - 2 >= book.currentPage && book.currentPage > 2)
        {
            isNext *= -1;
            text += 4;
            image += 2;
        }

        Debug.Log("FlipCheck: " + text + isRight);
    }

    public void Flipping()
    {
        StartCoroutine(auto.FlipToCurrentPage(DelayTime, AnimationFrame, BetweenTime, cPage));
    }

    public void StartTyping()
    {
        isEnd = true;
    }

    /*
    public void Enter()
    {
        btn.GetComponent<Image>().color = Color.white;
    }

    public void Exit()
    {
        btn.GetComponent<Image>().color = tmp;
    }
    */

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
