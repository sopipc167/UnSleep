using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadphonePanel : MonoBehaviour
{
    // 일단 급한대로 만듬. 캔버스로 바꾸기
    public Image image;
    public Text text;
    public GameObject tutorial;

    public float showTime = 3f;
    private bool playing = false;

    // Start is called before the first frame update
    void Update()
    {
        if (!tutorial.activeSelf && !playing)
        {
            StartCoroutine(showAndDisappear(showTime));
            playing = true;
        }
    }

   IEnumerator showAndDisappear(float showtime)
    {
        yield return new WaitForSeconds(showtime);

        Color icolor = image.color;
        Color tcolor = text.color;

        for (float alpha = 1f; alpha > 0f; alpha -= Time.deltaTime)
        {
            icolor.a = alpha;
            tcolor.a = alpha;

            image.color = icolor;
            text.color = tcolor;

            yield return null;
        }

        this.gameObject.SetActive(false);
    }
}
