using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Envelope : MonoBehaviour
{
    public Image En;
    public Image En_F;
    public GameObject CH;
    public Image En_B;

    public bool Click;
    public bool isOut;

    public Transform Pos;
    public Transform OriginalPos;

    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 1.0f;

    void Start()
    {
        GameObject.Find("Book").GetComponent<Book_test>()
            .OnFlip.AddListener(new UnityEngine.Events.UnityAction(onPageFlipped));

        En_B.gameObject.SetActive(false);
        CH.gameObject.SetActive(false);
    }

    private void onPageFlipped()
    {
        if (Click) // 페이지 넘겨질 때 봉투가 열려있으면 닫음.
        {
            CK();
        }
    }

    void Update()
    {
        if (isOut)
        {
            CH.transform.localPosition = Vector3.SmoothDamp(CH.transform.localPosition, Pos.localPosition, ref velocity, smoothTime);
        }
    }

    public void CK()
    {
        if (!Click)
        {
            En.gameObject.SetActive(false);
            En_F.gameObject.SetActive(true);
            En_B.gameObject.SetActive(true);
            CH.gameObject.SetActive(true);

            CH.transform.localPosition = OriginalPos.localPosition;

            isOut = true;
            Click = true;
        }
        else
        {
            En.gameObject.SetActive(true);
            En_B.gameObject.SetActive(false);
            En_F.gameObject.SetActive(false);
            CH.gameObject.SetActive(false);

            isOut = false;
            Click = false;
        }
    }

}
