using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phone56 : MonoBehaviour
{
    public string[] msgs;
    public Text text;
    public GameObject phone;
    public Button button;
    public CaveStopPanel caveStopPanel;

    Coroutine phone_co = null;
    private bool isRunning = false;


    private void Update()
    {
        //강제시청
        /*
                 if (Input.GetMouseButtonDown(0) && phone_co != null && isRunning)
        {
            StopCoroutine(phone_co);
            isRunning = false;
            StartCoroutine(PhoneYpos(0f, -1100f));
            button.enabled = true;
        }

         */
    }

    public void startPhone()
    {
        
        isRunning = true;
        phone_co = StartCoroutine(Phone());
    }


    IEnumerator Phone()
    {
        button.enabled = false;
        isRunning = true;
        caveStopPanel.dontShowDiaUI();
        phone.SetActive(true);

        yield return StartCoroutine(PhoneYpos(-1100f, 0f));

        yield return StartCoroutine(IBImsg());

        yield return StartCoroutine(PhoneYpos(0f, -1100f));

        phone.SetActive(false);
        isRunning = false;
        caveStopPanel.disableCaveStopPanel();
        button.enabled = true;
    }

    IEnumerator IBImsg()
    {
 

        for (int i = 0; i < msgs.Length; i++)
        {
            if (!isRunning)
            {
                text.text = "";
                yield break;
            }

            text.text = msgs[i];
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(1f);
    }

    IEnumerator PhoneYpos(float start, float end)
    {
        Vector2 curPos = new Vector2(0f, start);
        Vector2 endPos = new Vector2(0f, end);
        RectTransform RECT = phone.GetComponent<RectTransform>();

        while (Vector2.Distance(curPos, endPos) > 0.05f)
        {
            curPos = Vector3.Lerp(curPos, endPos, 3f * Time.deltaTime);
            RECT.anchoredPosition = curPos;

             yield return null;
        }


    }
}
