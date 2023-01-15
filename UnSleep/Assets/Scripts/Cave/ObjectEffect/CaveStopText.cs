using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaveStopText : MonoBehaviour
{

    public Text LineText;
    public string Line;


    private void OnEnable()
    {
        StartCoroutine(OnType(0.03f, Line));
    }

    IEnumerator OnType(float interval, string Line)
    {
        LineText.text = "";

        foreach (char item in Line)
        {
            LineText.text += item;
            yield return new WaitForSecondsRealtime(interval);
        }
    }
}
