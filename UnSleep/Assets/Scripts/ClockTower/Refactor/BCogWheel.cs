using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BCogWheel : CogWheel
{
    public BCogWheelInfo info;
    private SpeedPanel speedPanel = null;
    public bool satisfy
    {
        get
        {
            switch (info.type)
            {
                case BCogWheelType.START:
                    return true;
                case BCogWheelType.ROTATION:
                    return rotation == info.rotation;
                case BCogWheelType.SPEED:
                    return (int)speed == (int)info.speed;
                case BCogWheelType.ROTASPEED:
                    return rotation == info.rotation && (int)speed == (int)info.speed;
            }
            return false;
        }
    }
 

    private void Awake()
    {
        if (info.type == BCogWheelType.START)
        {
            state = CogState.ROTATE;
            speed = info.speed;
            rotation = info.rotation;
        }
    
    }

    public override void stop()
    {
        if (info.type == BCogWheelType.START) return;

        rotation = CogRotation.IDLE;
        speed = 0f;
        state = CogState.IDLE;
        if (speedPanel != null) speedPanel.updateBlackSpeedText(0f);
    }

    public override void receive(CogRotation r, float s, int l, float z)
    {
        rotation = r;
        speed = s;
        level = l;
        changeState(CogState.ROTATE);
        if (speedPanel != null) speedPanel.updateBlackSpeedText(s);
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }


    public void setSpeedPanel(SpeedPanel speedPanel)
    {
        this.speedPanel = speedPanel;
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
    START, ROTATION, SPEED, ROTASPEED
}