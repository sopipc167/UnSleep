using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class Gear_new : MonoBehaviour
{
    public int rotation; //1 : 반시계방향 , -1: 시계방향
    private float rotate_speed; //회전 속도
    public int teeth_num;
    public bool LastGear = false;
    public float initial_speed = 1200f;
    public GameObject Last_Gear;
    private float LastCnt = 0f;

    private void Start()
    {
        Last_Gear = GameObject.Find("LastGear");
        if (LastGear)
            initial_speed = 0f;
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, rotation * (initial_speed / teeth_num)) * Time.deltaTime);

        if (Last_Gear.GetComponent<Gear_Drag_new>().Stop)
        {
            LastCnt += Time.deltaTime;
        }
          
        if (LastCnt > 3f && initial_speed > 0f)
        {
            initial_speed -= Time.deltaTime*100f;
        }
    }

  

}
