using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NativeMove : MonoBehaviour
{
    public bool isMove;
    public int num1;
    public int num2;
    public bool isStop;

    void Start()
    {
        num1 = -1;
        num2 = -1;
    }


    void Update()
    {
        if (!isMove && !isStop)
        {
            num1 = Random.Range(1, 4);
            if(num1 != num2)
                StartCoroutine(Move());                
         }
    }

    IEnumerator Move()
    {
        isMove = true;
        Debug.Log("Move");
        if (num1 == 1)
            transform.DOMoveY(2.49f, 3).SetEase(Ease.InBounce);
        else if (num1 == 2)
            transform.DOMoveY(-0.06f, 3).SetEase(Ease.InBounce);
        else if (num1 == 3)
            transform.DOMoveY(-2.53f, 3).SetEase(Ease.InBounce);
        num2 = num1;
        yield return new WaitForSeconds(3.0f);
        isMove = false;
    }
}
