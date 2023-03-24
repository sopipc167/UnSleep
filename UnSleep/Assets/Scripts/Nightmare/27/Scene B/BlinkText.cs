using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkText : MonoBehaviour
{
    private Text text;

    private void OnEnable()
    {
        StartCoroutine(OnEnableCoroutine());
    }

    IEnumerator OnEnableCoroutine()
    {
        text = GetComponent<Text>();
        float alpha = 1f;
        float endTime;
        while (true)
        {
            endTime = Random.Range(0.05f, 0.15f);
            while (alpha > 0.001f)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
                alpha -= 1 / endTime * Time.deltaTime;
                yield return null;
            }
            endTime = Random.Range(0.05f, 0.15f);
            while (alpha < 0.999f)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
                alpha += 1 / endTime * Time.deltaTime;
                yield return null;
            }

            endTime = Random.Range(1.2f, 2f);
            while (endTime > 0f)
            {
                endTime -= Time.deltaTime;
                alpha += Random.Range(-0.05f, 0.05f);
                if (alpha > 1f) alpha = 1f;
                text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
                yield return null;
            }
        }
    }

}