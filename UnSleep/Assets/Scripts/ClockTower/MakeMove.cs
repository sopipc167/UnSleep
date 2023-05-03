using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MakeMove : MonoBehaviour
{

    public int num;
    public Image image;

    // Start is called before the first frame update
    void Start()
    {

        switch (num)
        {
            case 0: num0(); break;
            case 1: num1(); break;

            default: break;
        }
    }

    void Update()
    {
    }

    private void num0()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMoveX(-735f, 2f)).Append(image.DOColor(Color.green, 0.2f)).SetDelay(2f).SetLoops(-1, LoopType.Yoyo);
    }

    private void num1()
    {
        Sequence seq = DOTween.Sequence();
        RectTransform rect = GetComponent<RectTransform>();
        seq.Append(rect.DOAnchorPosY(-230f, 1f))
            .Append(transform.DOScale(0.8f, 0.5f))
            .Append(transform.DOScale(0.8f, 0.5f).SetDelay(2f))
            .SetLoops(-1, LoopType.Restart);
    }
}
