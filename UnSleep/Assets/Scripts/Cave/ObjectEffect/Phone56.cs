using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phone56 : MonoBehaviour
{
    public string[] msgs;
    public Text text;
    public GameObject phone;
    Coroutine phone_co = null;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && phone_co != null)
        {
            StopCoroutine(phone_co);
            phone.SetActive(false);
        }
    }

    public void startPhone()
    {
        phone_co = StartCoroutine(Phone());
    }


    IEnumerator Phone()
    {

        phone.SetActive(true);

        yield return StartCoroutine(PhoneYpos(-1100f, 0f));

        yield return StartCoroutine(IBImsg());

        yield return StartCoroutine(PhoneYpos(0f, -1100f));

        phone.SetActive(false);

    }

    IEnumerator IBImsg()
    {
        Debug.Log("여기");

        for (int i = 0; i < msgs.Length; i++)
        {
            text.text = msgs[i];
            yield return new WaitForSeconds(0.2f);
        }

        yield return null;
    }

    IEnumerator PhoneYpos(float start, float end)
    {
        Vector2 curPos = new Vector2(0f, start);
        Vector2 endPos = new Vector2(0f, end);
        RectTransform RECT = phone.GetComponent<RectTransform>();

        while (Vector2.Distance(curPos, endPos) > 0.05f)
        {
            curPos = Vector3.Lerp(curPos, endPos, 2f * Time.deltaTime);
            RECT.anchoredPosition = curPos;

             yield return null;
        }
    }
}
