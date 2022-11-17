using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstruction : MonoBehaviour
{
    public float speed_ori;
    public float speed;
    public bool isSpecial;
    public bool isCollision;
    public bool isStop;

    public GameObject Manager;

    void Start()
    {
        speed = speed_ori;
        Invoke("DestroyObject", 2.5f);
    }


    void Update()
    {
        if (!isCollision)
        {
            if (isSpecial)
                transform.position += new Vector3(speed_ori * Time.deltaTime, 0, 0);
            else
                transform.position -= new Vector3(speed_ori * Time.deltaTime, 0, 0);
        }
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(isSpecial)
                DestroyObject();
        }
    }
}
