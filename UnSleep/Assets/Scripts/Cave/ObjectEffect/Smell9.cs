using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Smell9 : MonoBehaviour
{
    private Image image;
    private RectTransform rect;
    private float tarPosY;
    public float Distance = 30f;
    public float Duration = 5f;

    void Start()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        tarPosY = rect.anchoredPosition.y - Distance;
        SmellAnimation();
    }



    void SmellAnimation()
    {
        image.DOFade(0f, Duration).SetLoops(-1, LoopType.Restart);
        rect.DOAnchorPosY(tarPosY, Duration).SetLoops(-1, LoopType.Restart);

    }

    private void OnEnable()
    {
        image.DORestart();
        rect.DORestart();
    }
}
