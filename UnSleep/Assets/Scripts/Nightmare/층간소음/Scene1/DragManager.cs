using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class DragManager : MonoBehaviour
{
    Vector3 mousePos, transPos, targetPos, originPos;
    public Canvas canvas;
    public Image[] it;
    public Image hand;
    public Image native;
    public bool isDrag;
    public bool isTag;
    public int iteamNum;
    public Animator[] iteamAnim;
    public Image injury;
    public Sprite[] handState;
    public int state;
    public RectTransform Background;
    public Color[] itColor;
    public bool isFall;
    public bool isLeaf;
    public bool isPoision;
    public bool isSnake;
    int k = 0;

    void Start()
    {
       for(int i = 0; i < 6; i++)
        {
            itColor[i] = it[i].color;
        }
    }


    void Update()
    {
        if (isDrag)
        {
            if (transPos.x < 155 && transPos.x > -205 && transPos.y > -447 && transPos.y < -35) //손 위치
            {
                isTag = true;
                it[iteamNum].color = new Vector4(255, 255, 255, 255);
                if (iteamNum == 1)
                    it[iteamNum].transform.localScale = new Vector3(1.5f, 1.5f, 1);
                else
                    it[iteamNum].transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                isTag = false;
                it[iteamNum].color = itColor[iteamNum];
                if (iteamNum == 1)
                    it[iteamNum].transform.localScale = new Vector3(1, 1, 1);
                else if(iteamNum == 3)
                    it[iteamNum].transform.localScale = new Vector3(0.3f, 0.3f, 1);
                else
                    it[iteamNum].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            }
        }

        if(isSnake && !isPoision && k < 3)
        {
            StartCoroutine(Poision());
            k++;
        }

        if (!isFall && !isLeaf)
        {
            StartCoroutine(Falling());
        }
    }

    public void GameSetting(bool ray)
    {
        for(int i = 0; i < it.Length; i++)
        {
            it[i].raycastTarget = ray;
        }
        hand.raycastTarget = ray;
        native.raycastTarget = ray;
    }

    IEnumerator Falling()
    {
        isFall = true;
        it[4].transform.DOLocalMoveY(-140, 1).SetEase(Ease.OutQuad, 10, 1);
        yield return new WaitForSeconds(1.5f);
        if (!isLeaf)
        {
            it[4].DOFade(0, 0.7f);
            yield return new WaitForSeconds(0.7f);
            it[4].transform.localPosition = new Vector3(330, 600, 0);
            it[4].color = new Vector4(1, 1, 1, 1);
        }
        isFall = false;
    }

    public void PointerUp()
    {
        if(isDrag)
            isTag = true;
    }

    public void IteamDragBegin(int a)
    {
        originPos = it[a].transform.position;
        iteamNum = a;
        if (iteamNum == 5)
            iteamAnim[iteamNum].SetTrigger("Stop");//뱀 움직임 멈추기

        if (iteamNum == 4)
            isLeaf = true;//잎 떨어지는 것 멈추기
    }

    public void IteamDrag()
    {
        isDrag = true; //아이템 드레그중
        transTargetPos(); //손 위치때문에
        targetPos = Input.mousePosition;
        it[iteamNum].transform.position = targetPos; //오브젝트가 움직일 때마다 마우스 위치로
    }

    public void IteamRelease()
    {
        if (isDrag && !isTag)
        {
            it[iteamNum].transform.DOMove(originPos, 1).SetEase(Ease.OutQuad);
            if (iteamNum == 4)
                Invoke("LeafFalling", 1.2f);
            else if (iteamNum == 5)
                iteamAnim[iteamNum].SetBool("isStop", false);

            isDrag = false;
        }
        else if (isTag) //아이템이 손 위치 안으로 들어왔을 때
        {
            isTag = false;
            
            switch (iteamNum)
            {
                case 0:
                    iteamAnim[iteamNum].SetTrigger("break");
                    StartCoroutine(Destory(0.7f, 0));
                    break;
                case 1:
                    iteamAnim[iteamNum].SetTrigger("break");
                    it[iteamNum].DOFade(0.5f, 0.7f);
                    StartCoroutine(Destory(0.5f, 1));
                    break;
                case 2:
                    iteamAnim[iteamNum].SetTrigger("eatSP");
                    StartCoroutine(Destory(1.0f, 2));
                    break;
                case 3:
                    it[iteamNum].transform.localPosition = new Vector3(-19, -346, 0);
                    it[iteamNum].transform.DOLocalMoveY(-150, 0.5f).SetLoops(5, LoopType.Restart);
                    StartCoroutine(Destory(3.0f, 3));
                    state = 3;
                    Invoke("changeState", 3.0f);
                    break;
                case 4:
                    it[iteamNum].DOFade(0, 1);
                    Invoke("healing", 1);
                    break;
                case 5:
                    it[iteamNum].DOFade(0, 0.7f);
                    StartCoroutine(Destory(0.5f, 5));
                    isSnake = true;
                    break;
            }

            isDrag = false;
        }
    }    

    public void LeafFalling()
    {
        isLeaf = false;
    }
    

    IEnumerator Poision()
    {
        isPoision = true;
        state = 1;
        changeState();
        
        yield return new WaitForSeconds(1);
        state = 2;
        changeState();
        hand.transform.DOLocalMoveX(-40, 0.5f).SetRelative().SetEase(Ease.OutFlash,10, 1);
        yield return new WaitForSeconds(0.5f);
        isPoision = false;
        if(k >= 3)
        {
            state = 0;
            changeState();
        }
    }

    public void changeState()
    {
        hand.sprite = handState[state];
        if(state == 3)
            StartCoroutine(NoiseManager.instance.GameClear());
    }

    public void healing()
    {
        injury.DOFade(0, 0.5f);
    }


    IEnumerator Destory(float w_time, int num)
    {
        yield return new WaitForSeconds(w_time);
        Destroy(it[num]);
    }

    public void Click()
    {
        native.transform.DOMoveX(60, 2).SetEase(Ease.Linear);
    }

    void transTargetPos()
    {
        mousePos = Input.mousePosition;
        Vector2 localPos = Background.InverseTransformPoint(mousePos);
        transPos = new Vector3(localPos.x,localPos.y, 0);
    }
}
