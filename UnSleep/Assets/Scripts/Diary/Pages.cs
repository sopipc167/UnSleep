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

    [Header ("페이지 요소")]
    public Text[] pageText;
    // 0->후일담1 / 1->제목1 / 2->요약1 / 3->후일담2 / 4->제목2 / 5->요약2

    public Button ImgButton12;
    public Button ImgButton34;
   

    public DiaryTextParser diaryTextParser; //일기장 텍스트 파싱하는 애
    public List<DiaryText> diaryText; //파싱된 데이터가 담김
    

    public bool isChange;
    int isNext = 1;
    //int j = 0;
    //int text = 0;
    //int image = 0;
    public bool isBack;

    public Cover cover;

    //public int epi;

    [Header("편지봉투")]
    public DiarySpriteManager DspriteManager; //일러스트 때려 넣을 애
    public GameObject charInfo; // 프리팹
    public GameObject CH12; //12페이지의 CH
    public GameObject CH34; //34페이지의 CH


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
            if (Dialogue_Proceeder.instance.End && !isStart)
            {
                Debug.Log("isEnd = true");
                auto.Mode = FlipMode.RightToLeft;
                cover.isBack = true;
                Invoke("Flipping", 1);
                isStart = true;
                Invoke("StartTyping", 1);

                Dialogue_Proceeder.instance.End = false;
            }
        }

        if (isEnd)
        {
            StartCoroutine(Typing(txt, message, speed)); //후일담 타이핑 효과
            isEnd = false;
        }

        if (!isChange && book.bookPages.Length - 2 > book.currentPage) //오른쪽으로 넘길 경우(페이지 업데이트)
        {
            //Debug.Log("isChange");
            //Debug.Log("text: " + text + "image: " + image + "isNext: " + isNext);
            //ChangePage();
            Re_ChangePage(true);

            isChange = true; 
            //Book_test.cs 361줄에서(오른쪽으로 넘기려고 클릭하면) isChange -> false
        }
        else
        {
            isChange = true;
        }

        if (!isBack && book.currentPage <= book.bookPages.Length - 2) //왼쪽으로 넘길 경우(페이지 전으로 돌리기)
        {
            /*
            Debug.Log("isBack " + text);
            페이지를 전과 같이 돌려놓기 위해서
            isNext *= -1;
            text -= 8;
            image -= 4;
            ChangePage();
            */
            Re_ChangePage(false);

            isBack = true;
        }
        else
        {
            isBack = true;
        }
    }

    // Re_ChangePage()로 대체
    /*
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
     */


    public void Re_ChangePage(bool Next) //True면 다음 페이지, False면 이전 페이지
    {
        // 일기장 내용을 에피소드 별로 관리 -> 현재 보고 있는 페이지가 어디 에피소드인지 알면 쉬워짐
        // Book_test의 CurrentPage/2 를 현재 페이지라고 할 수 있음 (2씩 올라가더라)
        // 홀수 장인 경우 page1-2를 보고 있을 것이고, 짝수 장인 경우 page3-4를 보고 있을 것이다.
        // CurrentPage/2: 홀수 = page3-4 갱신, 짝수 = page1-2 갱신

        int curPage = book.currentPage/2; //현재 보고 있던 페이지
        int curEpi = curPage-1; // epiID는 0부터 시작 
        if (Next)
            curEpi++;
        else
            curEpi--;

        if (curEpi < 0) curEpi = 0; //오류 방지


        if (curPage%2==0) // page1-2 갱신
        {
            pageText[0].text = diaryText[curEpi].afterstory; // 후일담
            pageText[1].text = diaryText[curEpi].epi_title; // 제목
            pageText[2].text = diaryText[curEpi].epi_intro; // 요약

            generateCh(curEpi, diaryText[curEpi].characs, true); // 편지봉투 내 등장인물 설명생성
        }
        else // page3-4 갱신
        {
            pageText[3].text = diaryText[curEpi].afterstory; // 후일담
            pageText[4].text = diaryText[curEpi].epi_title; // 제목
            pageText[5].text = diaryText[curEpi].epi_intro; // 요약

            generateCh(curEpi, diaryText[curEpi].characs, false); // 편지봉투 내 등장인물 설명생성
        }

        
    }

    public void FlipCheck(bool isRight)
    {

        if (isRight && book.bookPages.Length - 2 > book.currentPage && book.currentPage > 0)
        {
            isNext *= -1;
            //text -= 4;
            //image -= 2;
        }
        else if (!isRight && book.bookPages.Length - 2 >= book.currentPage && book.currentPage > 2)
        {
            isNext *= -1;
            //text += 4;
            //image += 2;
        }

        //Debug.Log("FlipCheck: " + text + isRight);
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
       
        if (Dialogue_Proceeder.instance != null)
        {
            Dialogue_Proceeder.instance.CurrentEpiID = cPage/2 -1 ;
            Dialogue_Proceeder.instance.isInit = true;
            Dialogue_Proceeder.instance.SetCurrentDiaID();
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

    public void generateCh(int epinum, CharacIntro[] characs, bool onetwo) //에피소드 별 등장인물 정보에 따라 생성. 마지막 bool은 페이지1-2면 true. 
    {
        


        // 인물 수에 따른 생성 위치, 각도 상수
        Dictionary<int, CHPosAngle[]> posDict
            = new Dictionary<int, CHPosAngle[]>()
        {
                {1 ,  new CHPosAngle[] { new CHPosAngle(80f, 0f, 0f) }},
                {2 ,  new CHPosAngle[] { new CHPosAngle(-30f, 0f, 10f), new CHPosAngle(150f, 0f, -10f)}},
                {3 ,  new CHPosAngle[] { new CHPosAngle(-70f, 0f, 10f), new CHPosAngle(80f, 0f, 0f), new CHPosAngle(200f, 0f, -10f)}},
                {4 ,  new CHPosAngle[] { new CHPosAngle(-140f, 0f, 10f), new CHPosAngle(0f, 0f, 5f), new CHPosAngle(115f, 0f, -5f), new CHPosAngle(250f, 0f, -10f)}},
                {5 ,  new CHPosAngle[] { new CHPosAngle(-165f, 0f, 10f), new CHPosAngle(-45f, 0f, 5f), new CHPosAngle(77f, 0f, 0f), new CHPosAngle(190f, 0f, -5f), new CHPosAngle(300f, 0f, -10f)}},
                {6 ,  new CHPosAngle[] { new CHPosAngle(-175f, 0f, 10f), new CHPosAngle(-55f, 0f, 5f), new CHPosAngle(30f, 0f, 0f), new CHPosAngle(80f, 0f, 0f), new CHPosAngle(2000f, 0f, -5f), new CHPosAngle(315f, 0f, -10f)}}
        };

        int chCnt = characs.Length; //인물 수
        Transform CH;

        //12페이지의 CH와 34페이지의 CH를 구분
        if (onetwo)
            CH = CH12.transform;
        else
            CH = CH34.transform;


        // 이미 있는건 삭제
        Transform[] childList = CH.GetComponentsInChildren<Transform>();
        if (childList != null)
        {
            for (int i = 1; i < childList.Length; i++) // 0부터 시작하면 부모 오브젝트도 삭제됨
            {
                if (childList[i] != CH)
                    Destroy(childList[i].gameObject);
            }
        }

        // 이미지 지정 - 해당 에피소드의 스프라이트를 들고 온다.
        DiarySprite CHsprite = DspriteManager.GetDiarySprite(epinum);

        // 이미지 들고 온 김에 시작 버튼 일러스트도 갈아낀다.
        if (epinum == SaveDataManager.Instance.Progress) // 클리어X 에피소드는 흑백 일러스트로
        {
            if (onetwo)
                ImgButton12.GetComponent<Image>().sprite = CHsprite.startSprB;
            else
                ImgButton34.GetComponent<Image>().sprite = CHsprite.startSprB;
        }
        else
        {
            if (onetwo)
                ImgButton12.GetComponent<Image>().sprite = CHsprite.startSpr;
            else
                ImgButton34.GetComponent<Image>().sprite = CHsprite.startSpr;
        }


        // 인물 수에 따라 CH에 생성시킨다. 
        for (int i = 0; i < chCnt; i++)
        {
            // 프리팹 생성, 부모 지정
            GameObject element = Instantiate(charInfo, CH);

            // 좌표, 각도 지정
            element.transform.localPosition = posDict[chCnt][i].pos;
            element.transform.Rotate(0, 0, posDict[chCnt][i].angle);

            // text 갱신
            element.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = characs[i].name;
            element.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text = characs[i].intro;

            // sprite 갱신
            element.GetComponent<Image>().sprite = CHsprite.charSpr[i];
        }

    }
}

public class CHPosAngle
{
    public Vector2 pos; // 생성 위치 
    public float angle; // -30 ~ 30의 각도
    public CHPosAngle(float x, float y, float ang) //생성자
    {
        pos = new Vector2(x, y);
        angle = ang;
    }
}