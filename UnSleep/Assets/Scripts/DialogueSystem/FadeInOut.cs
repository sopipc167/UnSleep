﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public GameObject FADEINOUT;
    public float FadeTime = 1f;
    Image FADE_panel;
    float start;
    float end;
    float time = 0f;
    bool isPlaying = false;



    public void Fade_InOut()
    {
        if (isPlaying == true) //중복방지 : 재생중이면 실행x
            return;
        FADE_panel = FADEINOUT.GetComponent<Image>();

        start = 0f;
        end = 1f;


        if (FADEINOUT.activeSelf == false)
        {
            FADEINOUT.SetActive(true);

        }

        StartCoroutine("fadein");

        start = 1f;
        end = 0f;
        StartCoroutine("fadeout");



    }

    IEnumerator fadeout()
    {

        isPlaying = true;
        time = 0f;
        Color color = FADE_panel.color;
        color.a = Mathf.Lerp(start, end, time);

        while(color.a > 0f)
        {
            time += Time.deltaTime / FadeTime;
            color.a = Mathf.Lerp(start, end, time);
            FADE_panel.color = color;

            yield return null;

        }
        isPlaying = false;
        FADEINOUT.SetActive(false);

    }

    IEnumerator fadein()
    {


        isPlaying = true;
        time = 0f;
        Color color = FADE_panel.color;
        color.a = Mathf.Lerp(start, end, time);
        
        while (color.a < 0.95f)
        {
            time += Time.deltaTime / FadeTime;
            color.a = Mathf.Lerp(start, end, time);
            FADE_panel.color = color;

            yield return null;

        }
        isPlaying = false;
    }

}