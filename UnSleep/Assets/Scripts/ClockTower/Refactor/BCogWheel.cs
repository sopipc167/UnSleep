using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BCogWheel : CogWheel
{
    public BCogWheelInfo info;

    private void Awake()
    {
        if (info.type == BCogWheelType.START)
        {
            speed = info.speed;
            rotation = info.rotation;
        }
    
    }
}

[System.Serializable]
public class BCogWheelInfo
{
    public BCogWheelType type; // START -> 시작 정보, OTHER -> 클리어 조건
    public float speed;
    public CogRotation rotation;
}

public enum BCogWheelType
{
    START, OTHER
}