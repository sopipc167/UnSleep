using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TugOfWar : MonoBehaviour
{
    public Slider Gauge;
    public GameObject Hand;
    public GameObject Foot;
    public bool isAdd;
    public bool isStart;
    public Image blood;
    public bool isEnd;
    public bool isOnce;
    public bool isFoot;

    public GameObject[] handChange;
    public GameObject[] footChange;

    void Start()
    {
        Gauge.value = 0.5f;
    }

    void Update()
    {
        if (isStart)
        {
            if (!isOnce)
            {
                isOnce = true;
                Gauge.gameObject.SetActive(true);
                Hand.gameObject.SetActive(true);
                //StartCoroutine(Timer());
            }
            if (!isAdd)
            {
                /*
                  if (isFoot)
                    StartCoroutine(Timer());
                */
                StartCoroutine(GaugeAdd());
            }

            if(Gauge.value == 0)
            {
                StartCoroutine(Clear());
            }
            else if(Gauge.value == 1.0f)
            {
                isStart = false;
                isEnd = true;
                Debug.Log("Game Over");
            }
        }
    }

    IEnumerator GaugeAdd()
    {
        isAdd = true;
        Gauge.value += 0.05f;
        yield return new WaitForSeconds(0.2f);
        isAdd = false;
    }

    public void ClickSpot()
    {
        //Debug.Log("Click");
        if(isStart)
            Gauge.value -= 0.05f;
    }

    IEnumerator Clear()
    {
        isStart = false;
        if (!isFoot)
        {
            handChange[0].SetActive(false);
            handChange[1].SetActive(true);
            yield return new WaitForSeconds(2.0f);
            Gauge.value = 0.5f;
            transform.localPosition = new Vector3(-121, 150, 0);
            Foot.SetActive(true);
            isFoot = true;
            yield return new WaitForSeconds(0.5f);
            isStart = true;
        }
        else
        {
            footChange[0].SetActive(false);
            footChange[1].SetActive(true);
            isEnd = true; 
            Debug.Log("Success");
        }
    }

    IEnumerator Timer()
    {
        int i = 0;

        while (!isEnd)
        {
            yield return new WaitForSeconds(1.0f);
            i++;
            if(i >= 10)
            {
                isStart = false;
                if(Gauge.value < 0.5f)
                {
                    if (!isFoot)
                    {
                        handChange[0].SetActive(false);
                        handChange[1].SetActive(true);
                        yield return new WaitForSeconds(2.0f);
                        Gauge.value = 0.5f;
                        transform.localPosition = new Vector3(-121, 150, 0);
                        Foot.SetActive(true);
                        isFoot = true;
                        yield return new WaitForSeconds(0.2f);
                        isStart = true;
                        break;
                    }
                    else
                    {
                        footChange[0].SetActive(false);
                        footChange[1].SetActive(true);
                        Debug.Log("Success");
                        break;
                    }
                }
                else
                {
                    Debug.Log("Game Over");
                    break;
                }
            }
        }
    }
}
