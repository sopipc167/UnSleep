using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowardPlayer : MonoBehaviour
{
    [Header("참조")]
    public Transform playerPos;

    private Vector3 basisPos;
    private Vector3 dirToTarget;

    // Update is called once per frame
    void Update()
    {
        basisPos = transform.position;
        basisPos.y = playerPos.position.y;

        dirToTarget = playerPos.position - basisPos;
        transform.rotation = Quaternion.LookRotation(dirToTarget, Vector3.up);
    }
}
