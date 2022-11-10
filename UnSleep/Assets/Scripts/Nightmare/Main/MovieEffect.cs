using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MovieEffect : MonoBehaviour
{
    public GameObject upper;
    public GameObject down;
    public bool isFramein;


    public void MovieFrameIn()
    {
        upper.SetActive(true);
        down.SetActive(true);
        upper.transform.DOLocalMoveY(561, 1.0f).SetEase(Ease.OutQuad);
        down.transform.DOLocalMoveY(-561, 1.0f).SetEase(Ease.OutQuad);
        isFramein = true;
    }

    public void MovieFrameout()
    {
        upper.transform.DOLocalMoveY(729, 1.0f).SetEase(Ease.OutQuad);
        down.transform.DOLocalMoveY(-729, 1.0f).SetEase(Ease.OutQuad);
        Invoke("FrameDisable", 1.0f);
    }

    public void FrameDisable()
    {
        upper.SetActive(false);
        down.SetActive(false);
        isFramein = false;
    }
}
