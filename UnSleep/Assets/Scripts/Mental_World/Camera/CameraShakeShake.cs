using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeShake : MonoBehaviour
{


    public void CamerShake(float duration, float magnitude)
    {
        StartCoroutine(CameraShakeCoroutine(duration, magnitude));
    }


    public IEnumerator CameraShakeCoroutine(float duration, float magnitude) //진동시간, 진동세기
    {
        float timer = 0f;
        Vector3 Start_Camera_Pos = transform.localPosition;

        while (timer <= duration)
        {
            transform.localPosition = (Vector3)Random.insideUnitSphere * magnitude + Start_Camera_Pos;
            timer += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = Start_Camera_Pos;
    }
  
}
