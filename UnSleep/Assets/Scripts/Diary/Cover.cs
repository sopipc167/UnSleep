using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Cover : MonoBehaviour
{
    Vector3 point = Vector3.zero;
    public RectTransform coverManager;

    public Book_test book;
    public Image bookPanel;
    public Image DragSpot_L;
    public Image DragSpot_R;

    public bool isDrag = true;
    public bool isStart;
    public Image Shadow;
    public Image shadowManager;

    public float duration = 0.3f;
    public bool isTween;
    public bool isEnd;
    public bool Ending;
    public float frameTime;

    float coverWidth;

    public Message M;

    public Image bookCover;

    public bool isBack;
    public bool back;

    public bool isOpen;

    void Start()
    {
        coverWidth = coverManager.rect.width;
        frameTime = 0.025f;
    }

    void Update()
    {
        //Debug.Log("x좌표: " + point.x + " coverWidth: " + coverWidth + " 비율: " + point.x / coverWidth);

        if (book.currentPage > 0 || book.drag == true && isDrag == true)
        {
            //Debug.Log("끄기");
            isDrag = false;
            shadowManager.gameObject.SetActive(false);
            DragSpot_L.gameObject.SetActive(false);
            transform.SetAsFirstSibling();
            bookPanel.transform.SetAsFirstSibling();
        }
        if (book.currentPage == 0 && book.drag == false && isDrag == false)
        {
            //Debug.Log("켜기");
            isDrag = true;
            transform.SetAsLastSibling();
            DragSpot_L.gameObject.SetActive(true);
            DragSpot_L.transform.SetAsLastSibling();
        }

        if (isDrag && isTween == false && isStart && !isEnd)
        {
            //Debug.Log((point.x / coverWidth) * 180);
            if(transform.localEulerAngles.y <= 180 && transform.localEulerAngles.y >= 0)
            {
                transform.localEulerAngles = new Vector3(0, ((point.x + 900) / coverWidth) * 180, 0);
                Shadow.transform.localPosition = new Vector3(point.x, Shadow.transform.localPosition.y, Shadow.transform.localPosition.z);

                if (point.x > 20)
                    bookCover.gameObject.SetActive(true);
                else
                    bookCover.gameObject.SetActive(false);
            }
        }

        if (isEnd)
        {
            StartCoroutine(TweenTo());
            isEnd = false;
        }

        if (isBack) //표지 auto 열림
        {
            back = true;
            StartCoroutine(TweenTo());
            isBack = false;
        }
    }

    public void UpdateCover(float p)
    {
        //Debug.Log("자동");
        transform.localEulerAngles = new Vector3(0, ((p + 900) / coverWidth) * 180, 0);
        Shadow.transform.localPosition = new Vector3(p, Shadow.transform.localPosition.y, Shadow.transform.localPosition.z);

        if (point.x > 0)
            bookCover.gameObject.SetActive(true);
        else
            bookCover.gameObject.SetActive(false);
    }

    public void CoverFilpStart()
    {
        isStart = true;
        shadowManager.gameObject.SetActive(true);
    }

    public void CoverLeftToRight()
    {
        if (isDrag)
        {
            Debug.Log(point); //마우스가 움직일 때만 증가하고,감소함 대신 빠르게 움직이면 세세하게 업데이트 하지 못함
            point = book.transformPoint(Input.mousePosition);
        }
    }

    public void ReleaseMouse()
    {
        Debug.Log("Release");
        if (!isTween)
        {
            isStart = false;
            StartCoroutine(TweenTo());
        }
    }


    public IEnumerator TweenTo()
    {
        isTween = true;
        int steps = (int)(duration / 0.025f);

        if (isEnd)
        {
            point.x = -700;
            frameTime = 0.015f;
            steps = (int)(duration / 0.005f);
            Ending = true;
        }

        if (back)
        {
            point.x = 910;
            frameTime = 0.025f;
            steps = (int)(duration / 0.015f);
        }

        if ((point.x > 0 || isEnd )&& !back) //엔딩연출 오토
        {
            //표지 닫힘
            Debug.Log("back: "+ back);
            float displacement = (1000 - point.x) / steps;
            float p = point.x + displacement;
            for (int i = 0; i < steps - 1; i++)
            {
                UpdateCover(p + displacement);
                p += displacement;

                yield return new WaitForSeconds(frameTime);
                point.x = p;
            }
            if(Ending)
                M.isEnd = true;

            DragSpot_L.gameObject.SetActive(false);
            DragSpot_R.gameObject.SetActive(true);
            book.LeftSpot.gameObject.SetActive(false);
            book.RightSpot.gameObject.SetActive(false);
        }
        else
        {
            //표지 열림
            float displacement = ((-850) - point.x) / steps;
            float p = point.x + displacement;
            for (int i = 0; i < steps - 1; i++)
            {
                UpdateCover(p + displacement);
                p += displacement;
                //Debug.Log(p + "TweenTo");

                yield return new WaitForSeconds(frameTime);
                point.x = p;
            }

            DragSpot_L.gameObject.SetActive(true);
            DragSpot_R.gameObject.SetActive(false);
            book.LeftSpot.gameObject.SetActive(true);
            book.RightSpot.gameObject.SetActive(true);
            shadowManager.gameObject.SetActive(false);
            back = false;
        }
        isTween = false;
    }
}
