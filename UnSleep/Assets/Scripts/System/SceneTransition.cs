using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public bool IsDone { private set; get; } = true;

    private Image img;
    private IEnumerator curCoroutine;

    private void Awake()
    {
        img = transform.GetChild(0).GetComponent<Image>();
    }

    public void FadeOut(float delay = 1.5f)
    {
        IsDone = false;
        img.gameObject.SetActive(true);
        if (curCoroutine != null) StopCoroutine(curCoroutine);
        curCoroutine = FadeOutCoroutine(delay);
        StartCoroutine(curCoroutine);
    }

    public void FadeIn(float delay = 1.5f)
    {
        IsDone = false;
        img.gameObject.SetActive(true);
        if (curCoroutine != null) StopCoroutine(curCoroutine);
        curCoroutine = FadeInCoroutine(delay);
        StartCoroutine(curCoroutine);
    }

    private IEnumerator FadeOutCoroutine(float delay)
    {
        float tmp = 1f / delay;
        while (img.color.a < 0.99f)
        {
            img.color += new Color(0f, 0f, 0f, tmp * Time.unscaledDeltaTime);
            yield return Time.unscaledDeltaTime;
        }

        img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
        curCoroutine = null;
        IsDone = true;
    }

    private IEnumerator FadeInCoroutine(float delay)
    {
        float tmp = 1f / delay;
        while (img.color.a > 0.01f)
        {
            img.color -= new Color(0f, 0f, 0f, tmp * Time.unscaledDeltaTime);
            yield return Time.unscaledDeltaTime;
        }

        img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
        curCoroutine = null;
        IsDone = true;
        img.gameObject.SetActive(false);
    }
}
