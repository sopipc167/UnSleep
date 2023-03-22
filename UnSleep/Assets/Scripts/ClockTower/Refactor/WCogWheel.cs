using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WCogWheel : CogWheel
{
    private Vector3 offset;
    private bool dragging;

    private void Start()
    {
        radius = Vector2.Distance(transform.GetChild(0).position, transform.GetChild(1).position);
        action(detect());    
    }

    private void OnMouseDown()
    {
        offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    }

    private void OnMouseDrag()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - offset;
        dragging = true;
        stop();
        action(detect());
    }

    private void OnMouseUp()
    {
        dragging = false;
        action(detect());
    }

    private bool rotationValidation(CogWheel[] cogWheels)
    {

        CogWheel[] adjoinWheels = cogWheels.filter(cw => getCogAction(cw) == CogAction.ADJOIN);
        if (adjoinWheels.Length < 2) return true;

        CogRotation criteria = adjoinWheels[0].rotation;

        foreach (CogWheel cw in adjoinWheels)
        {
            if (cw.rotation != criteria) return false;
        }

        return true;
    }

    private void action(CogWheel[] cogWheels)
    {
        if (!rotationValidation(cogWheels))
        {
            // 비활성화, 빨강
            return;
        }

        foreach (CogWheel cw in cogWheels)
        {
 
            switch (getCogAction(cw))
            {
                case CogAction.ADJOIN:
                    switch (state)
                    {
                        case CogState.ROTATE: 
                            if (!dragging) 
                                givePower(cw); 
                            break;
                        case CogState.IDLE:
                            if (!dragging)
                                getPower(cw);
                            break; 
                    }
                    break;
                case CogAction.FAR:
                    if (cw.isAlone()) cw.stop();
                    break;
                case CogAction.RESTRICT:
                    break;
                case CogAction.OVERLAP:
                    break;
            }
            
        }
    }





}
