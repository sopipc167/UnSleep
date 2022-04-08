using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusManager : LakeMovement
{
    internal bool canMove = false;
    internal bool isRight = false;
    internal bool isLeft = false;

    public void Stop()
    {
        isRight = false;
        isLeft = false;
        velocity = 0f;
    }

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
                transform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + velocity));
                Accelerate();
            }
            else if (isLeft)
            {
                transform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - velocity));
                Accelerate();
            }
            else
            {
                velocity = 0f;
            }
        }
    }
}
