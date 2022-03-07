using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowLakeClear : MonoBehaviour
{
    public Image mainImg;
    public Color changeColor;


    private void OnEnable()
    {
        StartCoroutine(ClearCoroutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator ClearCoroutine()
    {
        mainImg.color = Color.white;
        Color tmp = Color.white - changeColor;
        tmp /= 0.7f;

        while (true)
        {
            while (mainImg.color.b > changeColor.b)
            {
                mainImg.color -= tmp * Time.deltaTime;
                yield return null;
            }
            mainImg.color = changeColor;

            while (mainImg.color.b < 0.99f)
            {
                mainImg.color += tmp * Time.deltaTime;
                yield return null;
            }
            mainImg.color = Color.white;
        }
    }
}
