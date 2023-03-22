using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WCogWheel : CogWheel
{
    private Vector3 offset;

    private void Start()
    {
        spriteManager = GetComponent<CogWheelSpriteManager>();
        radius = Vector2.Distance(transform.GetChild(0).position, transform.GetChild(1).position);
        actionUp(detect());    
    }

    private void OnMouseDown()
    {
        offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    }

    private void OnMouseDrag()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - offset;
        stop();
        actionDrag(detect());
    }

    private void OnMouseUp()
    {
        actionUp(detect());
    }

    private bool rotationValidation(CogWheel[] cogWheels)
    {

        CogWheel[] adjoinWheels = filterCogWheelsByCogActionType(cogWheels, CogAction.ADJOIN);
        if (adjoinWheels.Length < 2) return true;

        CogRotation criteria = adjoinWheels[0].rotation;

        foreach (CogWheel cw in adjoinWheels)
        {
            if (cw.rotation != criteria) return false;
        }

        return true;
    }

    private CogWheel[] filterCogWheelsByCogActionType(CogWheel[] cogWheels, CogAction action)
    {
        return cogWheels.Filter(cw => getCogAction(cw) == action);
    }

    private void actionDrag(CogWheel[] cogWheels)
    {
        if (cogWheels.isEmtpy()) return;

        if (!rotationValidation(cogWheels))
        {
            if (spriteManager != null)
                spriteManager.setColor(CogAction.RESTRICT);
            return;
        }

        CogAction nearestCogAction = getCogAction(cogWheels[0]);
        Debug.Log(cogWheels[0].name);
        if (spriteManager != null)
            spriteManager.setColor(nearestCogAction);
    }


    private void actionUp(CogWheel[] cogWheels)
    {
        if (cogWheels.isEmtpy()) return;

        if (!rotationValidation(cogWheels))
        {
            if (spriteManager != null)
                spriteManager.setColor(CogAction.RESTRICT);
            return;
        }

        if (spriteManager != null)
            spriteManager.setColor(Color.white);

        switch (getCogAction(cogWheels[0]))
        {
            case CogAction.ADJOIN:
                if (state == CogState.IDLE)
                {
                    getPower(cogWheels[0]);
                }
                else if (state == CogState.ROTATE)
                {
                    CogWheel[] adjoinCw = filterCogWheelsByCogActionType(cogWheels, CogAction.ADJOIN);
                    foreach (CogWheel cw in adjoinCw) givePower(cw);
                }
                break;
            case CogAction.FAR:
                break;
            case CogAction.RESTRICT:
                inactive();
                break;
            case CogAction.OVERLAP:
                overlap(cogWheels[0]);
                CogWheel[] overlapAdjoinCw = filterCogWheelsByCogActionType(cogWheels, CogAction.ADJOIN);
                foreach (CogWheel cw in overlapAdjoinCw) givePower(cw);
                break;
        }


        CogWheel[] farCw = filterCogWheelsByCogActionType(cogWheels, CogAction.FAR);
        foreach (CogWheel cw in farCw) if (cw.isAlone()) cw.stop();

    }






}
