using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class Paper27 : MonoBehaviour
{
    
    private Image image;
    private RectTransform rect;
    private float tarPosY;
    public float direction = 1f;


    void Start()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        tarPosY = rect.anchoredPosition.y - 500f;

        FallingPaper();

    }



    void FallingPaper()
    {
        image.DOFade(0f, 7f).SetLoops(-1, LoopType.Restart);
        rect.DOPunchAnchorPos(new Vector2(400f* direction, 0), 3f, 0, 0).SetLoops(-1, LoopType.Restart);
        rect.DOAnchorPosY(tarPosY, 7f).SetLoops(-1, LoopType.Restart);

    }
}
