using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LakeMovement : MonoBehaviour
{
    internal float velocity = 0f;
    private float acceleration;
    private float rotationAcceleration;
    private float rotationVelocity;

    protected void InitAccel(float _acceleration, float _rotationAcceleration = 0f)
    {
        acceleration = _acceleration;
        rotationAcceleration = _rotationAcceleration;
    }

    protected virtual void Accelerate()
    {
        if (velocity < acceleration * 80)
        {
            velocity += acceleration;
        }
    }

    protected void SlowdownRotate()
    {
        if (velocity < acceleration * 20)
        {
            velocity -= acceleration;
        }
        else
        {
            velocity *= 0.8f;
        }
    }

    protected void SlowdownLine()
    {
        if (velocity < acceleration * 12)
        {
            velocity -= acceleration;
        }
        else
        {
            velocity *= 0.5f;
        }
    }
     
    protected void RotateBall(Transform ballPos, bool isLeft, bool isSlow)
    {
        if (isSlow)
        {
            if (rotationVelocity < rotationAcceleration * 20)
            {
                rotationVelocity -= rotationAcceleration;
            }
            else
            {
                rotationVelocity *= 0.8f;
            }
        }
        else
        {
            if (rotationVelocity < rotationAcceleration * 20)
            {
                rotationVelocity += rotationAcceleration;
            }
        }

        if (isLeft)
        {
            ballPos.localEulerAngles += new Vector3(0, 0, -rotationVelocity);
        }
        else
        {
            ballPos.localEulerAngles += new Vector3(0, 0, rotationVelocity);
        }
    }
}