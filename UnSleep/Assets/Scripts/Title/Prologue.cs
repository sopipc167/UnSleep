using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Prologue : MonoBehaviour
{
    private string[] scripts = {"반가워. 넌 이름이 뭐야?",
        "넌 어떤 사람이니?", "좋아하는 계절이 뭐야?",
        "이중에서 어떤 말을 듣고 싶어?", "마지막으로 너의 꿈을 얘기해줄래?"};

    private YourInfo yourInfo; //정보

    public Text LineText; //질문
    private bool isTyping;
    private Coroutine coroutine;

    public InputField nameField; //이름 입력창
    public InputField dreamField; //꿈 입력창

    public GameObject[] Answers; //입력 오브젝트들
    private int Ansidx = 0;

    public GameObject Really;
    public Text ques;
    public Text input;


    void Start()
    {
        yourInfo = new YourInfo();
        coroutine = StartCoroutine(OnType(0.05f, scripts[Ansidx]));
        StartCoroutine(TurnOnAnsw());
    }


    public void GoNext()
    {
        Answers[Ansidx].SetActive(false);
        Ansidx++;

        if (Ansidx < 5)
        {
            coroutine = StartCoroutine(OnType(0.05f, scripts[Ansidx]));
            StartCoroutine(TurnOnAnsw());

        }
        else
        {
            Saveinfo();
            SceneManager.LoadScene("Diary");
            //Test();
        }
    }


    public void Test()
    {
        YourInfo loadinfo = SaveDataManager.Instance.LoadYourInfo();
        Debug.Log(loadinfo.name + loadinfo.person.ToString() + loadinfo.season.ToString() + loadinfo.message.ToString() + loadinfo.dream);
    }

    public void ComfirmName() //이름 입력하고 확인 누르면
    {
        yourInfo.name = nameField.text;
        Really.SetActive(true);
        RealComfirm();
    }

    public void ComfirmDream() //꿈 입력하고 확인 누르면
    {
        yourInfo.dream = dreamField.text;
        Really.SetActive(true);
        RealComfirm();
    }


    public void RealComfirm() //바꿀 수 없다고 창 띄우고 확인
    {
        if (Ansidx == 0) //이름
        {
            ques.text = "정말 이게 네 이름이니?";
            input.text = yourInfo.name;
        }
        else if (Ansidx == 4) //꿈
        {
            ques.text = "이 꿈이 맞니?";
            input.text = yourInfo.dream;

        }
    }

    public void Yes()
    {
        Really.SetActive(false);
        GoNext();

    }

    public void No()
    {
        Really.SetActive(false);
    }


    public void Select() //버튼 선택
    {
        PrologueBtn selectedBtn = EventSystem.current.currentSelectedGameObject.GetComponent<PrologueBtn>();

        switch (Ansidx) //질문 종류에 따라 저장
        {
            case 1: yourInfo.person = selectedBtn.Getinfo(); break;
            case 2: yourInfo.season = selectedBtn.Getinfo(); break;
            case 3: yourInfo.message = selectedBtn.Getinfo(); break;
            default: break;
        }

        GoNext();


    }

    public void Saveinfo() //저장
    {
        SaveDataManager.Instance.SaveYourInfo(yourInfo);
    }


    IEnumerator OnType(float interval, string Line)
    {
        isTyping = true;
        LineText.text = "";

        foreach (char item in Line)
        {
            LineText.text += item;
            yield return new WaitForSeconds(interval);
        }
        isTyping = false;
    }

    IEnumerator TurnOnAnsw()
    {
        yield return new WaitForSeconds(1f);
        Answers[Ansidx].SetActive(true);
    }


}
