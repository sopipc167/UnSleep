using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public bool IsDone { private set; get; } = true;

    private Image img;
    private IEnumerator curCoroutine;
    private bool isClicked = false;

    private void Awake()
    {
        img = transform.GetChild(0).GetComponent<Image>();
    }

    public void FadeOut(float delay = 1.5f)
    {
        IsDone = false;
        img.gameObject.SetActive(true);
        curCoroutine = FadeOutCoroutine(delay);
        StartCoroutine(curCoroutine);
    }

    public void FadeIn(float delay = 1.5f)
    {
        IsDone = false;
        img.gameObject.SetActive(true);
        curCoroutine = FadeInCoroutine(delay);
        StartCoroutine(curCoroutine);
    }

    public void FadeOut(Image curImg, Sprite nextSprite, float delay)
    {
        IsDone = false;
        img.gameObject.SetActive(true);
        curCoroutine = FadeOutCoroutine(curImg, nextSprite, delay);
        StartCoroutine(curCoroutine);
    }

    public void Dissolve(Image curImg, Sprite nextSprite, float delay)
    {
        IsDone = false;
        img.gameObject.SetActive(true);
        curCoroutine = DissolveCoroutine(curImg, nextSprite, delay);
        StartCoroutine(curCoroutine);
    }

    private IEnumerator FadeOutCoroutine(float delay)
    {
        float tmp = 1f / delay;
        while (img.color.a < 0.99f && !isClicked)
        {
            img.color += new Color(0f, 0f, 0f, tmp * Time.deltaTime);
            yield return null;
        }

        isClicked = false;
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
        curCoroutine = null;
        IsDone = true;
    }

    private IEnumerator FadeInCoroutine(float delay)
    {
        float tmp = 1f / delay;
        while (img.color.a > 0.01f && !isClicked)
        {
            img.color -= new Color(0f, 0f, 0f, tmp * Time.deltaTime);
            yield return null;
        }

        isClicked = false;
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
        curCoroutine = null;
        IsDone = true;
        img.gameObject.SetActive(false);
    }

    private IEnumerator FadeOutCoroutine(Image curImg, Sprite nextSprite, float delay)
    {
        float tmp = 1f / delay;
        while (img.color.a < 0.99f && !isClicked)
        {
            img.color += new Color(0f, 0f, 0f, tmp * Time.deltaTime);
            yield return null;
        }

        isClicked = false;
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
        curImg.sprite = nextSprite;
        curCoroutine = null;
        IsDone = true;
    }

    private IEnumerator DissolveCoroutine(Image curImg, Sprite nextSprite, float delay)
    {
        img.material = curImg.material;
        img.sprite = nextSprite;
        img.color = new Color(255f, 255f, 255f, 0f);

        float tmp = 1 / delay;
        while (img.color.a < 0.99f && !isClicked)
        {
            img.color += new Color(0f, 0f, 0f, tmp * Time.deltaTime);
            yield return null;
        }

        isClicked = false;
        img.color = new Color(curImg.color.r, curImg.color.g, curImg.color.b, 1f);
        curImg.sprite = img.sprite;
        curCoroutine = null;
        IsDone = true;
        img.gameObject.SetActive(false);
    }
}
