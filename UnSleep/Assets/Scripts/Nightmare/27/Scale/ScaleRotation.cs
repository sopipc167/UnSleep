﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleRotation : MonoBehaviour
{
    [Header("수치 1당 이동하는 양 조절")]
    public float size;

    [Header("저울 초기화")]
    public Transform mObj;
    public Transform sObj1;
    public Transform sObj2;

    private Quaternion target;

    public void RotateScale(int value, float delay = 1f)
    {
        StopAllCoroutines();
        StartCoroutine(RotateCoroutine(value, delay));
    }

    private IEnumerator RotateCoroutine(int value, float delay)
    {
        target = Quaternion.Euler(new Vector3(0f, 0f, -value * size));
        Quaternion sTargetRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        float timeCnt = 0f;
        float tmp = 1f / delay;

        while (timeCnt < 1f)
        {
            mObj.transform.rotation = Quaternion.Slerp(mObj.transform.rotation, target, timeCnt);
            sObj1.transform.rotation = Quaternion.Slerp(sObj1.transform.rotation, sTargetRotation, timeCnt);
            sObj2.transform.rotation = Quaternion.Slerp(sObj2.transform.rotation, sTargetRotation, timeCnt);
            timeCnt += Time.deltaTime * tmp;
            yield return null;
        }

        mObj.transform.rotation = target;
        sObj1.transform.rotation = sTargetRotation;
        sObj2.transform.rotation = sTargetRotation;
    }
}
