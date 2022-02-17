using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetObjType { Cave, ClockTower, Lake, Cliff, Volcano }

public class TargetObjManager : MonoBehaviour
{
    [Header("목표제시 오브젝트")]
    public TargetObj cave;
    public TargetObj clockTower;
    public TargetObj lake;
    public TargetObj cliff;
    public TargetObj volcano;

    //임시 데이터
    bool flag;

    public void SetOrder(params TargetObjType[] list)
    {
        StartCoroutine(SetOrderCoroutine(list));
    }

    public IEnumerator SetOrderCoroutine(params TargetObjType[] list)
    {
        foreach (var elem in list)
        {
            TargetObj targetObj;
            switch (elem)
            {
                case TargetObjType.Cave:
                    targetObj = cave;
                    break;
                case TargetObjType.ClockTower:
                    targetObj = clockTower;
                    break;
                case TargetObjType.Lake:
                    targetObj = lake;
                    break;
                case TargetObjType.Cliff:
                    targetObj = cliff;
                    break;
                case TargetObjType.Volcano:
                    targetObj = volcano;
                    break;
                default:
                    targetObj = null;
                    break;
            }
            targetObj.SetTarget();

            //bool 받아서 코루틴 제어
            yield return new WaitUntil(() => flag);

            targetObj.StopTarget();
        }
    }
}
