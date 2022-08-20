using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NightmareOpenning : MonoBehaviour
{
    
    private RectTransform rectTransform;
    private Image image;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();


        Sequence sequence = DOTween.Sequence();

        sequence.SetDelay(2f)
            .Append(image.DOColor(new Color(1f, 1f, 1f), 2f))
            .Append(rectTransform.DOAnchorPosY(-540f, 7f, true))
            .OnComplete(() =>
           {
               transform.parent.gameObject.SetActive(false);
           });

    }


   
}
