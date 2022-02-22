using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MakeMove : MonoBehaviour
{

    public int num;

    // Start is called before the first frame update
    void Start()
    {
        switch (num)
        {
            case 0: num0(); break;
            case 1: num1(); break;

            default: break;
        }
    }

    void Update()
    {
        Debug.Log(transform.position);
    }

    private void num0()
    {
        transform.DOMoveX(-734f, 2f).SetDelay(2f).SetLoops(-1, LoopType.Yoyo);
    }

    private void num1()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMoveY(484.5f, 1.5f)).Append(transform.DOMoveY(480.7f, 0.3f)).SetLoops(-1, LoopType.Restart);
    }
}
