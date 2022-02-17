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
        //Debug.Log(transPos);
        if (isDrag)
        {
            if (transPos.x < 155 && transPos.x > -205 && transPos.y > -447 && transPos.y < -35)
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
        Debug.Log("PointerUP");
        if(isDrag)
            isTag = true;
    }

    public void IteamDragBegin(int a)
    {
        //Debug.Log("Begin");
        originPos = it[a].transform.position;
        iteamNum = a;
        if (iteamNum == 5)
            iteamAnim[iteamNum].SetTrigger("Stop");

        if (iteamNum == 4)
            isLeaf = true;
    }

    public void IteamDrag()
    {
        //Debug.Log("Drag");
        isDrag = true;
        transTargetPos();
        targetPos = Input.mousePosition;
        it[iteamNum].transform.position = targetPos;
    }

    public void IteamRelease()
    {
        //Debug.Log("End");
        if (isDrag && !isTag)
        {
            isDrag = false;
            it[iteamNum].transform.DOMove(originPos, 1).SetEase(Ease.OutQuad);
            if (iteamNum == 4)
                Invoke("LeafFalling", 1.2f);
        }
        else if (isTag)
        {
            isTag = false;
            isDrag = false;
            switch (iteamNum)
            {
                case 0:
                    iteamAnim[iteamNum].SetTrigger("break");
                    Invoke("Destroy", 0.7f);
                    break;
                case 1:
                    iteamAnim[iteamNum].SetTrigger("break");
                    it[iteamNum].DOFade(0.5f, 0.7f);
                    Invoke("Destroy", 0.5f);
                    break;
                case 2:
                    iteamAnim[iteamNum].SetTrigger("eatSP");
                    Invoke("Destroy", 1.0f);
                    break;
                case 3:
                    it[iteamNum].transform.localPosition = new Vector3(-19, -346, 0);
                    it[iteamNum].transform.DOLocalMoveY(-150, 0.5f).SetLoops(5, LoopType.Restart);
                    Invoke("Destroy", 3.0f);
                    state = 3;
                    Invoke("changeState", 3.0f);
                    break;
                case 4:
                    it[iteamNum].DOFade(0, 1);
                    Invoke("healing", 1);
                    break;
                case 5:
                    it[iteamNum].DOFade(0, 0.7f);
                    Invoke("Destroy", 0.5f);
                    isSnake = true;
                    break;
            }
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
    }

    public void healing()
    {
        injury.DOFade(0, 0.5f);
    }

    public void Destroy()
    {
        Destroy(it[iteamNum]);
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
