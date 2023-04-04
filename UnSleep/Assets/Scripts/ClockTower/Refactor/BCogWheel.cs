using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BCogWheel : MonoBehaviour, CogWheel
{
    public CogWheelInfo info;
    public BCogWheelInfo bInfo;
    private SpeedPanel speedPanel = null;

    public bool satisfy
    {
        get
        {
            switch (bInfo.type)
            {
                case BCogWheelType.START:
                    return true;
                case BCogWheelType.ROTATION:
                    return info.rotation == bInfo.rotation;
                case BCogWheelType.SPEED:
                    return (int)info.speed == (int)bInfo.speed;
                case BCogWheelType.ROTASPEED:
                    return info.rotation == bInfo.rotation && (int)info.speed == (int)bInfo.speed;
            }
            return false;
        }
    }
 

    private void Awake()
    {
        if (bInfo.type == BCogWheelType.START)
        {
            info.state = CogState.ROTATE;
            info.speed = bInfo.speed;
            info.rotation = bInfo.rotation;
            info.level = 1;
        }
    
    }
    void Start()
    {
        info.radius = Vector2.Distance(transform.GetChild(0).position, transform.GetChild(1).position);
    }

    void Update()
    {
        if (info.state == CogState.ROTATE)
        {
            transform.Rotate(new Vector3(0f, 0f, info.speed * (int)info.rotation * 0.01f));
        }
    }

    public void setSpeedPanel(SpeedPanel speedPanel)
    {
        this.speedPanel = speedPanel;
    }

    public void givePower(CogWheel other)
    {
        CogWheelInfo otherInfo = other.getCogWheelInfo();
        if (info.state == CogState.INACTIVE || info.state == CogState.IDLE ||
            otherInfo.state == CogState.ROTATE || otherInfo.state == CogState.OVERLAP) return;

        other.receive(info, transform.position.z);
    }

    public void getPower(CogWheel other)
    {
        CogWheelInfo otherInfo = other.getCogWheelInfo();
        if (info.state == CogState.ROTATE || info.state == CogState.OVERLAP ||
            otherInfo.state == CogState.INACTIVE || otherInfo.state == CogState.IDLE) return;

        receive(otherInfo, other.getPosition().z);
    }


    public void receive(CogWheelInfo otherInfo, float z)
    {
        changeState(CogState.ROTATE, otherInfo);
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
        if (speedPanel != null) speedPanel.updateBlackSpeedText(info.speed);
    }


    public void stop()
    {
        if (bInfo.type == BCogWheelType.START) return;

        info.rotation = CogRotation.IDLE;
        info.speed = 0f;
        info.state = CogState.IDLE;
        if (speedPanel != null) speedPanel.updateBlackSpeedText(0f);
    }

    public void idle()
    {
        changeState(CogState.IDLE);
    }

    public CogWheel[] detect()
    {
        List<CogWheel> cogWheels = new List<CogWheel>();
        cogWheels.AddRange(FindObjectsOfType<BCogWheel>());
        cogWheels.AddRange(FindObjectsOfType<WCogWheel>());
        return CogWheelUtil.sortByDistance(this, cogWheels.ToArray().Filter(cw => cw.getCogWheelInfo().state != CogState.INACTIVE));
    }

    public bool isAlone()
    {
        foreach (CogWheel cw in detect())
        {
            if (CogWheelUtil.getCogAction(this, cw) != CogAction.FAR)
                return false;
        }
        return true;

    }

    public void changeState(CogState newState, CogWheelInfo otherInfo = null)
    {
        info.update(newState, otherInfo);
    }

    public bool hasOverlap()
    {
        return false;
    }

    public Vector3 getPosition()
    {
        return transform.position;
    }

    public CogWheelInfo getCogWheelInfo()
    {
        return info;
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