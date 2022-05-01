using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBG : MonoBehaviour
{
    Color babyyellow = new Color(242f/255f, 243f/255f, 228f/255f);
    Color coral = new Color(255f/255f, 216f/255f, 216f/255f);
    Color skyblue = new Color(235f/255f, 255f/255f, 249f/255f);
    Color pastelgreen = new Color(238f/255f, 253f/255f, 230f/255f);
    Color lavender = new Color(245f/255f, 230f/255f, 253f/255f);
    public float duration = 5.0f;
   
    void Start()
    {
        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        StartCoroutine("LC");
    }


    IEnumerator LC()
    {
        float t=0f;
        while (t/duration < 1f)
        {
            Camera.main.backgroundColor = Color.Lerp(lavender, coral, t/duration);
            t += 0.1f;
            yield return new WaitForSeconds(.1f);
        }
        yield return StartCoroutine("CY");
    }

    IEnumerator CY()
    {
        float t = 0f;
        while (t/duration < 1f)
        {
            Camera.main.backgroundColor = Color.Lerp(coral, babyyellow, t/duration);
            t += 0.1f;
            yield return new WaitForSeconds(.1f);
        }
        yield return StartCoroutine("YG");
    }

    IEnumerator YG()
    {
        float t = 0f;
        while (t / duration < 1f)
        {
            Camera.main.backgroundColor = Color.Lerp(babyyellow, pastelgreen, t / duration);
            t += 0.1f;
            yield return new WaitForSeconds(.1f);
        }
        yield return StartCoroutine("GB");
    }

    IEnumerator GB()
    {
        float t = 0f;
        while (t / duration < 1f)
        {
            Camera.main.backgroundColor = Color.Lerp(pastelgreen,skyblue, t / duration);
            t += 0.1f;
            yield return new WaitForSeconds(.1f);
        }
        yield return StartCoroutine("BL");
    }

    IEnumerator BL()
    {
        float t = 0f;
        while (t / duration < 1f)
        {
            Camera.main.backgroundColor = Color.Lerp(skyblue, lavender, t / duration);
            t += 0.1f;
            yield return new WaitForSeconds(.1f);
        }
        yield return StartCoroutine("LC");
    }



}
