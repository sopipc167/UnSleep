using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gome : MonoBehaviour
{
    public GameObject target;
    public Vector3 targetPos;
    float speed = 4.5f;
    public float smooth;
    public bool isStart;
    public bool isMinigame;
    public Animator anim;
    public DiaEvent dia;
    public bool isFollow;

    void Start()
    {
        targetPos = transform.position;
    }

    void LateUpdate()
    {
        if (isStart)
        {
            if(transform.position == targetPos)
            {
                anim.SetBool("isMove", false);
                isStart = false;
                isFollow = false;
                dia.block.enabled = false;
            }
            else
            {
                anim.SetBool("isMove", true);

                if (isFollow)
                    targetPos = target.transform.position;

                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            }
        }
    }

    public void ChangeTarget(Vector3 pos)
    {
        targetPos = pos;
    }

    public void touchPlayer()
    {
        if (isMinigame)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            transform.position += new Vector3(2.5f, 0, 0);
            isMinigame = false;
        }
    }
}
