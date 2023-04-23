using System.Collections.Generic;
using UnityEngine;

public interface CogWheel
{
    void givePower(CogWheel other);
    void getPower(CogWheel other);
    void receive(CogWheelInfo info, float z);
    void stop();
    void idle();
    void reset();
    CogWheel[] detect();
    bool isAlone();
    void changeState(CogState newState, CogWheelInfo otherInfo, CogWheel cw);
    bool hasOverlap();
    CogWheelInfo getCogWheelInfo();
    Vector3 getPosition();
    void addChain(CogWheel cogWheel);
}

public enum CogRotation
{
    CLOCKWISE = -1, IDLE = 0, COUNTERCLOCKWISE = 1
}

public enum CogState
{
    ROTATE, INACTIVE, IDLE, OVERLAP, READY
}

public enum CogAction
{
    OVERLAP, RESTRICT, ADJOIN, FAR
}

[System.Serializable]
public class CogWheelInfo
{
    public CogState state;
    public CogRotation rotation;
    public float speed;
    public int size;
    public int level;
    public float radius;

    private CogRotation reverse(CogRotation cr)
    {
        switch (cr)
        {
            case CogRotation.CLOCKWISE:
                return CogRotation.COUNTERCLOCKWISE;
            case CogRotation.COUNTERCLOCKWISE:
                return CogRotation.CLOCKWISE;
            default:
                 return CogRotation.IDLE;
        }
    }

    public void update(CogState newState, CogWheelInfo otherInfo = null)
    {
        switch (newState)
        {
            case CogState.ROTATE:
                state = newState;
                rotation = reverse(otherInfo.rotation);
                speed = (otherInfo.speed * otherInfo.size) / size;
                level = otherInfo.level;
                break;
            case CogState.IDLE:
                state = newState;
                rotation = CogRotation.IDLE;
                speed = 0f;
                level = 0;
                break;
            case CogState.INACTIVE:
                state = newState;
                rotation = CogRotation.IDLE;
                speed = 0f;
                level = 0;
                break;
            case CogState.READY:
                state = newState;
                rotation = CogRotation.IDLE;
                speed = 0f;
                level = 0;
                break;
            case CogState.OVERLAP:
                state = newState;
                rotation = otherInfo.rotation;
                speed = otherInfo.speed;
                level = otherInfo.level + 1;
                break;
        }
    }

} 