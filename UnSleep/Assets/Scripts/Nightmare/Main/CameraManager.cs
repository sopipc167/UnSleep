using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public GameObject target;
    Vector3 targetPos;
    Vector3 originPos;
    public float smooth = 5.0f;

    public GameObject BG;
    public bool isStop;

    public bool isMiniGame;

    public bool isThree;
    Vector3 mousePos, transPos, targetPos_M;
    
    float FrameTime;
    public bool isFrame;

    public bool isStart;
    public bool isFirst;

    public TugOfWar Tug;
    public Image blood;

    public float delay;
    public float min;
    public float max;

    void Start()
    {
        if (isThree)
        {
            isFirst = true;
            targetPos = new Vector3(152.03f, 0.3f, -10);
        }
    }


    void Update()
    {
        if (!isStop && !isMiniGame && !isThree)
        {
            targetPos = new Vector3(target.transform.position.x, 0, -10);
        }
        else if (isMiniGame)
        {
            targetPos = new Vector3(-0.4f, 0, -10);
        }
        else if (isThree && !isFrame && !Tug.isStart && !Tug.isEnd)
            StartCoroutine(FrameWork());

        if(originPos != targetPos_M)
            transform.position = Vector3.Lerp(targetPos, transform.position, Time.deltaTime * smooth);
    }

    public void fadeOut()
    {
        blood.DOFade(0, 0.5f);
    }

    IEnumerator FrameWork()
    {
        isFrame = true;
        if (isFirst)
        {
            TransTargetPos();
            originPos = targetPos_M;
            yield return new WaitForSeconds(1.2f);
            isFirst = false;
        }


        TransTargetPos();
        if (targetPos_M.y < max && targetPos_M.y > min)
            targetPos = new Vector3(151.8f, targetPos_M.y / delay, -10);
        if (transform.position.y > 9.5f)
        {
            isStart = true;
            StartCoroutine(Timer());
        }
        else
        {
            isStart = false;
        }
        yield return new WaitForSeconds(FrameTime);
        isFrame = false;
    }

    IEnumerator Timer()
    {
        float i = 0;

        while (isStart)
        {
            i+=0.5f;
            yield return new WaitForSeconds(0.5f);

            if (i == 1.5f)
            {
                //blood.DOFade(255, 0.7f);
                //Invoke("fadeOut", 0.7f);
                Tug.gameObject.SetActive(true);
                Tug.isStart = true;
                isStart = false;
                break;
            }
        }
    }

    void TransTargetPos()
    {
        mousePos = Input.mousePosition;
        transPos = Camera.main.ScreenToWorldPoint(mousePos);

        targetPos_M = new Vector3(0, transPos.y, 0);
    }
}
