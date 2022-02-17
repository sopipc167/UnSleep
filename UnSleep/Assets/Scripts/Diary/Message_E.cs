using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message_E : MonoBehaviour
{
    public Message M;
    public Transform Pos;
    public float smoothTime = 0.5f;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        
    }

    void Update()
    {
        if (M.isClick)
        {
            transform.SetAsLastSibling();
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Pos.localPosition, ref velocity, smoothTime);
        }
    }
}
