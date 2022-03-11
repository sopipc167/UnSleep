using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tutorial3Effect : MonoBehaviour
{
    private Text text;

    void Start()
    {
        text = GetComponent<Text>();
        StartCoroutine(TextBlink());
    }

    IEnumerator TextBlink()
    {
        while (true)
        {
            float alpha = 0f;
            while (alpha < 1f)
            {
                alpha += Time.deltaTime;
                text.color = new Color(1f, 1f, 1f, alpha);
                yield return null;
            }
            text.color = new Color(1f, 1f, 1f, 1f);
            yield return null;

            while (alpha > 0f)
            {
                alpha -= Time.deltaTime;
                text.color = new Color(1f, 1f, 1f, alpha);
                yield return null;
            }

            text.color = new Color(1f, 1f, 1f, 0f);
            yield return null;

        }
    }
}
