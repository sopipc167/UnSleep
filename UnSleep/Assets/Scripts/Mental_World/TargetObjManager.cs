using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetObjType { Cave, ClockTower, Lake, Cliff, Volcano }

public class TargetObjManager : MonoBehaviour
{
    [Header("순서: 동굴, 시계, 호수, 절벽, 화산")]
    public TargetObj[] objs;

    [Header("1초동안 변화할 빛의 양")]
    public float deltaLight;

    [Header("빛 깜박임 변화정도")]
    public float maxVal;
    public float minVal;

    [Header("빛 설정")]
    public float lightRange;
    public Color lightColor;

    private bool flag = false;

    private void Awake()
    {
        foreach (var item in objs)
        {
            item.InitLight(lightRange, lightColor);
        }
    }

    public void NextTarget()
    {
        flag = true;
    }

    public void SetOrder(params TargetObjType[] list)
    {
        StartCoroutine(SetOrderCoroutine(list));
    }

    public IEnumerator SetOrderCoroutine(params TargetObjType[] list)
    {
        TargetObj targetObj;

        foreach (var elem in list)
        {
            switch (elem)
            {
                case TargetObjType.Cave:
                    targetObj = objs[0];
                    break;
                case TargetObjType.ClockTower:
                    targetObj = objs[1];
                    break;
                case TargetObjType.Lake:
                    targetObj = objs[2];
                    break;
                case TargetObjType.Cliff:
                    targetObj = objs[3];
                    break;
                case TargetObjType.Volcano:
                    targetObj = objs[4];
                    break;
                default:
                    targetObj = null;
                    break;
            }
            targetObj.SetTarget(deltaLight, maxVal, minVal);

            yield return new WaitUntil(() => flag);
            flag = false;

            targetObj.StopTarget(deltaLight);
        }
    }
}
