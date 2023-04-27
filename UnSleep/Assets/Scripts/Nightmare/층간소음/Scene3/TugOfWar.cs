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

    public TextManager TM;
    public FadeInOut fadeinout;
    public Image BG_2;

    public RectTransform Background;

    public RectTransform top;
    public RectTransform bottom;

    public bool isNoise;
    public bool isScissor;
    public AudioClip noiseSound;

    int i = 0;

    void Start()
    {
        Gauge.value = 0.5f;
        isNoise = true;
    }

    void Update()
    {
        if (isNoise && !isScissor)
            StartCoroutine(Noise());

        if (isStart)
        {
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
                i++;
                StartCoroutine(GameOver());
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
            if (limit >= 1.3f)
            {
                //TugOfWar 실행조건
                isScissor = true;
                isMouseMove = false;
                BG.enabled = false;
                isCount = false;
                Gauge.gameObject.SetActive(true);
                Hand.gameObject.SetActive(true);
                SoundManager.Instance.PlayBGM("Extreme Fear");
                TM.DiaUI.SetActive(true);
                TM.Increasediaindex = true;
            }
        }
    }

    IEnumerator Noise()
    {
        isNoise = false;
        yield return new WaitForSeconds(3.0f);
        SoundManager.Instance.PlaySE(noiseSound);
        yield return new WaitForSeconds(0.5f);
        isNoise = true;
    }

    IEnumerator GameOver()
    {
        fadeinout.Blackout_Func(0.5f);
        TW.raycastTarget = false;
        yield return new WaitForSeconds(0.7f);
        Gauge.value = 0.5f;
        if(i >= 2)
            Dialogue_Proceeder.instance.UpdateCurrentDiaID(2007);

        TM.DiaUI.SetActive(true);
        TM.Increasediaindex = true;
    }

    void TransMousePos()
    {
        mousePos = Input.mousePosition;
        transPos_M = Camera.main.ScreenToWorldPoint(mousePos);
    }


    IEnumerator bgUp()
    {
        isMove = true;

        if (BG.transform.position.y < bottom.transform.position.y)
        {
            targetPos = BG.transform.position + new Vector3(0, frame, 0);
            BG.transform.position = Vector3.Lerp(targetPos, BG.transform.position, Time.deltaTime);
            frame -= 0.3f;
         }

        yield return new WaitForSeconds(frameTime);
        isMove = false;
    }

    IEnumerator bgDown()
    {
        isMove = true;

        if (BG.transform.position.y > top.transform.position.y)
        {
            isCount = false;
            targetPos = BG.transform.position - new Vector3(0, frame, 0);
            BG.transform.position = Vector3.Lerp(targetPos, BG.transform.position, Time.deltaTime);
            frame += 0.3f;
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
        yield return new WaitForSeconds(0.3f);
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
            StartCoroutine(Ending());
        }
    }

    IEnumerator Ending()
    {
        SoundManager.Instance.FadeOutBGM(delay:1.0f);
        fadeinout.Blackout_Func(0.5f);
        BG_2.enabled = true;
        yield return new WaitForSeconds(0.5f);
        Dialogue_Proceeder.instance.UpdateCurrentDiaID(2008);
        TM.DiaUI.SetActive(true);
        TM.Increasediaindex = true;
    }
}
