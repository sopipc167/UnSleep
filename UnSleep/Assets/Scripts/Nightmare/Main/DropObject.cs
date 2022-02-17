using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObject : MonoBehaviour
{
    Vector3 endPos;
    public float speed;

    void Start()
    {
        float tmp = transform.position.y - 11;
        endPos = new Vector3(transform.position.x, tmp, 0);
    }

    private void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPos, Time.deltaTime * speed);
    }
}
