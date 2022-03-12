using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SceneTransEffectManager : MonoBehaviour
{
    
    public FadeInOut fadeinout;
    public GameObject blink;


    private void Start()
    {
        fadeinout = GetComponent<FadeInOut>();
        
    }

    public void FadeInOut()
    {
        fadeinout.Fade_InOut();
    }

    public void WaitBlackOut(float waitsec)
    {
        fadeinout.Blackout_Func(waitsec);
    }

    public void BlinkOpen()
    {
        //blink = GetComponent<BlinkAnimation>();

        blink.GetComponent<BlinkAnimation>().BlinkOpen();
    }


    public void BlinkClose()
    {
        //blink = GetComponent<BlinkAnimation>();
        blink.GetComponent<BlinkAnimation>().BlinkClose();
    }
}
