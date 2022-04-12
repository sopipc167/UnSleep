using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMan : MonoBehaviour
{
    public float MoveSpeed;
    public Transform playerPos;

    private Vector3 pos;

    // Update is called once per frame
    void Update()
    {
        //pos = transform.position;
        //transform.position += (playerPos.position - pos) * MoveSpeed;
    }
}
