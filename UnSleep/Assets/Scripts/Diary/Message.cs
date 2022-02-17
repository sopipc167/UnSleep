using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    public Text M;
    public string Player;
    public string People;
    public string Weather;
    public string Word;
    public string Dream;

    public Transform Pos;
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    public Auto auto;
    public Book_test book;

    public Image Message_E;
    public Text M_Expansion;
    public Transform Pos_E;

    public bool isClick = false;
    public bool isEnd;

    void Start()
    {
        M.text = "Dear. " + Player + "\n\n안녕. 너에게 이렇게 인사하는 건 처음인 것 같아. 이 편지가 너에게 잘 전달되었으면 좋겠다.\n" +
            "하고 싶은 말은 많은데, 너무 많이 적으면 혹여나 이 편지가 무거워 너에게 전달되지 않을까...그런 걱정에 길게 적지 못했어.\n지금 생각하면 정말 엉뚱한 생각이었던 것 같네!\n" +
            "있잖아... 너는 정말 " + People + "이었어. 나는 그런 " + Weather + " 모습이 너무나 좋았어.\n그동안 정말로 " + Word + "\n늘 " + Dream + " (이)라는 너의 꿈. 이룬다면 좋겠다.\n"
            + "처음이자 마지막으로 보내는 응원이라는 게 아쉬울 정도로 너를 응원해.\n" + "그럼 잘 자. 그리고 잘 가. 내 오랜 친구야.\n";
    }

    void Update()
    {
        if (isEnd && book.currentPage == 0)
        {
            transform.SetAsLastSibling();
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Pos.localPosition, ref velocity, smoothTime);
        }
    }

    public void Click()
    {
        Debug.Log("Click");
        gameObject.SetActive(false);
        M_Expansion.text = "Dear. " + Player + "\n\n안녕. 너에게 이렇게 인사하는 건 처음인 것 같아. 이 편지가 너에게 잘 전달되었으면 좋겠다.\n\n" +
            "하고 싶은 말은 많은데, 너무 많이 적으면 혹여나 이 편지가 무거워 너에게 전달되지 않을까...그런 걱정에 길게 적지 못했어.\n\n지금 생각하면 정말 엉뚱한 생각이었네\n\n" +
            "있잖아... 너는 정말 " + People + "이었어. 나는 그런 " + Weather + " 모습이 너무나 좋았어.\n\n그동안 정말로 " + Word + "\n\n늘 " + Dream + " (이)라는 너의 꿈. 이룬다면 좋겠다.\n"
            + "처음이자 마지막으로 보내는 응원이라는 게 아쉬울 정도로 너를 응원해.\n\n" + "그럼 잘 자. 그리고 잘 가. 내 오랜 친구야.\n";
        isClick = true;
    }
}
