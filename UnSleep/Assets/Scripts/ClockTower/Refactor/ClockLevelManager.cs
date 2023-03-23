using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockLevelManager : MonoBehaviour
{
    public BCogWheel[] bCogWheels;

    void Start()
    {
        bCogWheels = FindObjectsOfType<BCogWheel>();
    }

    private void checkClear()
    {
        if (bCogWheels.Count(bcw => !bcw.satisfy) == 0)
        {
            Debug.Log("클리어~");
        } else
        {
        }
    }

    // Update is called once per frame
    void Update()
    {
        checkClear();
    }
}
