using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject[] Gears;
    void Start()
    {
        Gears = GameObject.FindGameObjectsWithTag("Gear");

    }

    public void ResetAll()
    {
        Gears = GameObject.FindGameObjectsWithTag("Gear");

        for (int i = 0; i < Gears.Length; i++)
        {
            if (!Gears[i].GetComponent<Gear>().Static_Gear)
            {
                Gears[i].GetComponent<Gear_Drag>().GoToStartPos();
            }
        }
    }

}
