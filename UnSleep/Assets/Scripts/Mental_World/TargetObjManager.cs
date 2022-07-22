using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        foreach (var item in objs)
        {
            item.InitLight(lightRange, lightColor);
        }
    }

    private void Start()
    {
        SetTarget(Dialogue_Proceeder.instance.CurrentPuzzle);
    }

    public void SetTarget(string targetName)
    {
        TargetObj targetObj;
        switch (targetName)
        {
            case "Cave":
                targetObj = objs[0];
                break;
            case "ClockTower":
                targetObj = objs[1];
                break;
            case "Lake":
                targetObj = objs[2];
                break;
            case "Cliff":
                targetObj = objs[3];
                break;
            default:
                targetObj = objs[4];
                break;
        }
        targetObj.SetTarget(deltaLight, maxVal, minVal);
    }
}
