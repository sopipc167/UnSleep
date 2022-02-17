using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//마우스 인식을 위한 콜라이더 필요
[RequireComponent(typeof(SphereCollider))]

public class RotateLake : MonoBehaviour
{
    internal bool isRotating = false;
    internal bool isStart = false;
    internal bool moveWithMouseWheel = false;

    private readonly Quaternion[] destinations = new Quaternion[7];

    //Mouse Wheel
    private int keyPos = 0;

    //Mouse Drag
    private const float halfAngle = 360 / 14f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 7; i++)
        {
            destinations[i] = Quaternion.Euler(new Vector3(0f, 0f, 360 * i / 7f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStart)
        {
            //마우스 휠 동작 (드래그 동작은 LakeManager에서 관리)
            if (moveWithMouseWheel)
            {
                if (isRotating)
                {
                    float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
                    if (scroll > 0)
                    {
                        //Counterclockwise
                        ++keyPos;
                    }
                    else if (scroll < 0)
                    {
                        //Clockwise
                        --keyPos;
                    }
                }
                transform.rotation = Quaternion.Slerp(transform.rotation, GetDestinationByKey(), Time.deltaTime * 20f);
            }
            else if (!isRotating)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, GetDestinationByPos(), Time.deltaTime * 20f);
            }
        }
    }

    private Quaternion GetDestinationByKey()
    {
        if (keyPos > 6) keyPos = 0;
        else if (keyPos < 0) keyPos = 6;
        return destinations[keyPos];
    }

    private Quaternion GetDestinationByPos()
    {
        float keyRotation = transform.eulerAngles.z;
        float keyAngle = 0f;
        float plus, minor;

        for (int i = 0; i < 7; i++)
        {
            keyAngle = destinations[i].eulerAngles.z;
            minor = keyAngle - halfAngle;
            plus = keyAngle + halfAngle;
            if (minor < 0f)
            {
                minor += 360f;
                if (minor < keyRotation || keyRotation <= plus)
                {
                    keyPos = i;
                    return destinations[i];
                }
            }

            if (minor < keyRotation && keyRotation <= plus)
            {
                keyPos = i;
                return destinations[i];
            }
        }

        return Quaternion.Euler(new Vector3(0, 0, keyAngle));
    }
}