using System.Collections.Generic;
using UnityEngine;

public interface CogWheel
{
    void givePower(CogWheel other);
     void getPower(CogWheel other);
     void receive(CogRotation r, float s, int l, float z);
     void stop();
     void idle();
     CogWheel[] detect();
     bool isAlone();
     void changeState(CogState newState);
     bool hasOverlap();
     CogWheelInfo getCogWheelInfo();
     Vector3 getPosition();
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

    public void updateInfo(CogState cs, CogRotation cr, float sp, int l)
    {
        state = cs;
        rotation = cr;
        speed = sp;
        level = l;
    }

    public void updateInfo(CogState cs)
    {
        state = cs;
    }
} 