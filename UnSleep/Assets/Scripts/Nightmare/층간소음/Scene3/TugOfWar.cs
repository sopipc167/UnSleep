using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TugOfWar : MonoBehaviour
{
    public Image TW;
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

    public Image BG;
    public RectTransform screen;
    new Vector3 mousePos;
    new Vector3 transPos_M;
    new Vector3 targetPos;
    public bool isMove;
    public bool isMouseMove;
    public float frame;
    public float frameTime;

    public bool isCount;
    float time;
    float limit;

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
            }
            if (!isAdd)
            {
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

        TransMousePos();
        if (!isMove && isMouseMove)
        {
            if (transPos_M.y >= 2.0f)
                StartCoroutine(bgDown());
            else
                StartCoroutine(bgUp());
        }

        //Timer
        if (isCount)
        {
            time += Time.deltaTime;
            limit = ((int)time % 60);
            if (limit >= 1.5f)
            {
                isMouseMove = false;
                TW.raycastTarget = true;
                BG.enabled = false;
                isStart = true;
                isCount = false;
            }
        }
    }

    void TransMousePos()
    {
        mousePos = Input.mousePosition;
        transPos_M = Camera.main.ScreenToWorldPoint(mousePos);
    }

    IEnumerator bgUp()
    {
        isMove = true;

        if(BG.transform.position.y < 430)
        {
            targetPos = BG.transform.position + new Vector3(0, frame, 0);
            BG.transform.position = Vector3.Lerp(targetPos, BG.transform.position, Time.deltaTime);
            Debug.Log(BG.transform.position.y);
        }

        yield return new WaitForSeconds(frameTime);
        isMove = false;
    }

    IEnumerator bgDown()
    {
        isMove = true;

        if(BG.transform.position.y > 10)
        {
            isCount = false;
            targetPos = BG.transform.position - new Vector3(0, frame, 0);
            BG.transform.position = Vector3.Lerp(targetPos, BG.transform.position, Time.deltaTime);
            Debug.Log(BG.transform.position.y);
        }
        else
        {
            isCount = true;
        }

        yield return new WaitForSeconds(frameTime);
        isMove = false;
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
}
