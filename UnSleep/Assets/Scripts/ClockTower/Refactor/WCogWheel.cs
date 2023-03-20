﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WCogWheel : CogWheel
{
    private Vector3 offset;

    private void OnMouseDown()
    {
        offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    }

    private void OnMouseDrag()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - offset;
        detect();
    }

    private void OnMouseUp()
    {
        detect();
    }

    private void detect()
    {
        CogWheel[] cogWheels = sortByDistance(FindObjectsOfType<CogWheel>());
        foreach (CogWheel cw in cogWheels)
        {
            Debug.Log(getCogAction(cw));
            switch (getCogAction(cw))
            {
                case CogAction.ADJOIN:
                    switch (state)
                    {
                        case CogState.ROTATE: givePower(cw); break;
                        case CogState.IDLE: getPower(cw); break;
                    }
                    break;
                case CogAction.FAR:
                    cw.stop();
                    break;
                case CogAction.RESTRICT:
                    break;
                case CogAction.OVERLAP:
                    break;
            }
        }
    }



}
