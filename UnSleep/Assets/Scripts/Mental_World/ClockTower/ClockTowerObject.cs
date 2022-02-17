using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockTowerObject : MonoBehaviour
{
    public GameObject Gear1;
    public GameObject Gear2;
    public GameObject Gear3;
    public GameObject Gear4;
    public GameObject Gear5;
    public GameObject Gear6;
    public GameObject chu1;
    public GameObject chu2;
    public GameObject timeniddle;
    public GameObject minniddle;


    void Update()
    {
        Gear1.transform.Rotate(0, 0, 0.6f);
        Gear2.transform.Rotate(0, 0, -0.15f);
        Gear3.transform.Rotate(0, 0, 0.15f);
        Gear4.transform.Rotate(0, 0, -0.1f);
        Gear5.transform.Rotate(0, 0, 0.1f);
        Gear6.transform.Rotate(0, 0, -0.05f);

        timeniddle.transform.Rotate(0, 0, 0.08f);
        minniddle.transform.Rotate(0, 0, 0.007f);
    }
}
