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

        switch (info.rotation)
        {
            case CogRotation.CLOCKWISE:
                other.receive(CogRotation.COUNTERCLOCKWISE, (info.speed * info.size) / otherInfo.size, info.level, transform.position.z);
                break;
            case CogRotation.COUNTERCLOCKWISE:
                other.receive(CogRotation.CLOCKWISE, (info.speed * info.size) / otherInfo.size, info.level, transform.position.z);
                break;
        }
    }

    public void getPower(CogWheel other)
    {
        CogWheelInfo otherInfo = other.getCogWheelInfo();
        if (info.state == CogState.ROTATE || info.state == CogState.OVERLAP ||
            otherInfo.state == CogState.INACTIVE || otherInfo.state == CogState.IDLE) return;

        switch (otherInfo.rotation)
        {
            case CogRotation.CLOCKWISE:
                receive(CogRotation.COUNTERCLOCKWISE, (otherInfo.speed * otherInfo.size) / info.size, otherInfo.level, other.getPosition().z);
                break;
            case CogRotation.COUNTERCLOCKWISE:
                receive(CogRotation.CLOCKWISE, (otherInfo.speed * otherInfo.size) / info.size, otherInfo.level, other.getPosition().z);
                break;
        }
    }

    public void receive(CogRotation r, float s, int l, float z)
    {
        info.rotation = r;
        info.speed = s;
        info.level = l;
        changeState(CogState.ROTATE);
        if (speedPanel != null) speedPanel.updateBlackSpeedText(s);
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
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

    public void changeState(CogState newState)
    {
        info.state = newState;
        switch (newState)
        {
            case CogState.IDLE:
                info.rotation = CogRotation.IDLE;
                info.speed = 0f;
                info.level = 0;
                break;
            case CogState.INACTIVE:
                info.rotation = CogRotation.IDLE;
                info.speed = 0f;
                info.level = 0;
                break;
        }
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