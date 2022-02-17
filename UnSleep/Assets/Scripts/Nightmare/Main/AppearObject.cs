using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearObject : MonoBehaviour
{
    public float speed;

    void Start()
    {
        Invoke("destroy", 0.8f);
    }

    void Update()
    {
        transform.position += new Vector3(Time.deltaTime * speed, 0, 0);
    }

    void destroy()
    {
        Destroy(gameObject);
    }
}
