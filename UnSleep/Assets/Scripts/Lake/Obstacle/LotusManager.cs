using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusManager : LakeMovement
{
    internal bool canMove = false;
    internal bool isRight = false;
    internal bool isLeft = false;

    private void Awake()
    {
        InitAccel(0.06f);
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            if (isRight)
            {
                transform.eulerAngles += Vector3.forward * velocity;
                Accelerate();
            }
            else if (isLeft)
            {
                transform.eulerAngles += Vector3.back * velocity;
                Accelerate();
            }
            else
            {
                velocity = 0f;
            }
        }
    }
}
