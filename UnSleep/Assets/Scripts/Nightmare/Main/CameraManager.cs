using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public RectTransform screen;
    public GameObject target;
    public Vector3 targetPos;
    Vector3 originPos;
    public float smooth = 5.0f;

    public GameObject BG;
    public bool isStop;

    public bool isMiniGame;
    public bool isThree;
    public bool isStart;

    public TugOfWar Tug;
    public Image blood;

    public bool isChoose;

    void Start()
    {
        originPos = new Vector3(500, 500, 500);
        targetPos = new Vector3(0, 0, -10);
    }


    void Update()
    {
        if (!isStop && !isMiniGame)
        {
            if(target.transform.position.x >= -0.6 && target.transform.position.x <= 19.45)
            {
                if (!isChoose)
                    targetPos = new Vector3(target.transform.position.x, 0, -10);
                else
                    targetPos = new Vector3(target.transform.position.x + 5.5f, 0, -10);
            }
        }
        else if (isMiniGame)
        {
            targetPos = new Vector3(-0.4f, 0, -10);
        }

        transform.position = Vector3.Lerp(targetPos, transform.position, Time.deltaTime * smooth);
    }

    public void fadeOut()
    {
        blood.DOFade(0, 0.5f);
    }

    
}
