using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public GameObject FADEINOUT;
    public float FadeTime;
    public Image FADE_panel;
    bool isPlaying = false;

    private void Awake()
    {
        FADE_panel = FADEINOUT.GetComponent<Image>();
    }

    public void Blackout_Func(float waitsec)
    {

        if (FADEINOUT.activeSelf == false)
        {
            FADEINOUT.SetActive(true);

        }


        StartCoroutine("Blackout_Co", waitsec);

    }

    public void FadeStop(bool isStart)
    {
        if (isStart)
        {
            Debug.Log("fadein");
            StartCoroutine(fadein());
        }
        else
        {
            StartCoroutine(fadeout());
        }
    }

    IEnumerator Blackout_Co(float waitsec)
    {

        Color _color = FADE_panel.color;
        _color.a = 1f;

        FADE_panel.color = _color;

        yield return new WaitForSeconds(waitsec);

        StartCoroutine("fadeout");
    }


    public void Fade_InOut()
    {
        if (isPlaying == true) //중복방지 : 재생중이면 실행x
            return;
        //FADE_panel = FADEINOUT.GetComponent<Image>();


        if (FADEINOUT.activeSelf == false)
        {
            FADEINOUT.SetActive(true);

        }
        StartCoroutine("fadeinout");
    }

    public void Fade_Out()
    {

        if (FADEINOUT.activeSelf == false)
        {
            FADEINOUT.SetActive(true);

        }
        StartCoroutine("fadeout");
    }

    public void Fade_In()
    {


        if (FADEINOUT.activeSelf == false)
        {
            FADEINOUT.SetActive(true);

        }
        StartCoroutine("fadein");
    }

    IEnumerator fadeinout()
    {
     
        yield return StartCoroutine("fadein");

        yield return StartCoroutine("fadeout");
    }

    IEnumerator fadeout()
    {

        isPlaying = true;
        float time = 0f;
        float start = 1f;
        float end = 0f;
        Color color = FADE_panel.color;
        color.a = Mathf.Lerp(start, end, time);

        while(color.a > 0f)
        {
            time += Time.deltaTime / FadeTime;
            color.a = Mathf.Lerp(start, end, time);
            FADE_panel.color = color;

            yield return null;

        }
        color.a = 0f;
        isPlaying = false;
        FADEINOUT.SetActive(false);

    }

    IEnumerator fadein()
    {
        isPlaying = true;
        float time = 0f;
        float start = 0f;
        float end = 1f;
        Color color = FADE_panel.color;
        color.a = Mathf.Lerp(start, end, time);
        
        while (color.a < 0.95f)
        {
            time += Time.deltaTime / FadeTime;
            color.a = Mathf.Lerp(start, end, time);
            FADE_panel.color = color;

            yield return null;

        }
        color.a = 1f;
        isPlaying = false;
    }

}
