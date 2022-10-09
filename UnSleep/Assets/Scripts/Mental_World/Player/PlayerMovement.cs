using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("잠재우미 이동속도 설정")]
    public float speed;

    [Header("잠재우미 공중에 떠있는 크기")]
    public float addY;

    public bool MovePlayer(Vector3 destination)
    {
        destination.y += addY;
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        return Vector3.Distance(transform.position, destination) > 0.1f;
    }
}
