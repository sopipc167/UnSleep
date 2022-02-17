using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMan : MonoBehaviour
{
    public float MoveSpeed;

    private Transform Target;
    private Vector3 Pos;

    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Pos = transform.position;
        transform.position += (Target.position - Pos) * MoveSpeed;
    }
}
