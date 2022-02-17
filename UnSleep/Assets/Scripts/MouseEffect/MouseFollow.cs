using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    private float distance;
    private Camera mainCam;
    private Vector3 prePos, curPos;
    private Ray ray;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        distance = -mainCam.transform.position.z;
        prePos = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        curPos = Input.mousePosition;
        if (curPos != prePos)
        {
            prePos = curPos;
            ray = mainCam.ScreenPointToRay(curPos);
            transform.position = ray.GetPoint(distance);
        }
    }
}
