using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : LakeMovement
{
    [Header("공 클래스")]
    public Transform ball;

    [Header("회전 가속도"), Range(0.01f, 0.05f)]
    public float rotateAcceleration;

    [Header("회전 공 회전가속도"), Range(0.05f, 0.5f)]
    public float lineRotationAcceleration;

    internal bool isRight = false;
    internal bool isLeft = false;
    internal bool isRightStop = false;
    internal bool isLeftStop = false;

    private void Awake()
    {
        InitAccel(rotateAcceleration, lineRotationAcceleration);
    }

    private void FixedUpdate()
    {
        if (isRight)
        {
            transform.eulerAngles += new Vector3(0, 0, velocity);
            Accelerate();
            RotateBall(ball, false, false);
        }
        else if (isLeft)
        {
            transform.eulerAngles += new Vector3(0, 0, -velocity);
            Accelerate();
            RotateBall(ball, true, false);
        }
        else if (isRightStop)
        {
            transform.eulerAngles += new Vector3(0, 0, -velocity);
            SlowdownRotate();
            RotateBall(ball, true, true);
        }
        else if (isLeftStop)
        {
            transform.eulerAngles += new Vector3(0, 0, velocity);
            SlowdownRotate();
            RotateBall(ball, false, true);
        }
        else
        {
            velocity = 0f;
        }

        if (velocity < 0f)
        {
            velocity = 0f;
            isRightStop = false;
            isLeftStop = false;
        }
    }
}