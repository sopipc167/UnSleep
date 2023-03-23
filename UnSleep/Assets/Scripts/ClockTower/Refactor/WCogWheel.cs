using UnityEngine;

public class WCogWheel : CogWheel
{
    private Vector3 offset;
    private const float lerpSpeed = 5f;

    private void Start()
    {
        spriteManager = GetComponent<CogWheelSpriteManager>();
        radius = Vector2.Distance(transform.GetChild(0).position, transform.GetChild(1).position);
        actionUp(detect());
    }

    private void OnMouseDown()
    {
        offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        if (state == CogState.INACTIVE) state = CogState.IDLE;
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

    void Update()
    {
        switch (state)
        {
            case CogState.ROTATE: 
                transform.Rotate(new Vector3(0f, 0f, speed * (int)rotation * 0.01f)); 
                break;
            case CogState.INACTIVE: 
                transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(0.5f, 0.5f), Time.deltaTime * lerpSpeed); 
                break;
            case CogState.IDLE:
                transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(1f, 1f), Time.deltaTime * lerpSpeed); 
                break;
        }

    }


    private bool rotationValidation(CogWheel[] cogWheels)
    {

        CogWheel[] adjoinWheels = filterCogWheelsByCogActionType(cogWheels, CogAction.ADJOIN);
        if (adjoinWheels.Length < 2) return true;

        CogRotation criteria = adjoinWheels[0].rotation;

        foreach (CogWheel cw in adjoinWheels)
        {
            if (cw.rotation == CogRotation.IDLE) continue;
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

        if (spriteManager != null)
            spriteManager.setColor(nearestCogAction);
    }


    private void actionUp(CogWheel[] cogWheels)
    {
        if (cogWheels.isEmtpy()) return;

        if (!rotationValidation(cogWheels))
        {
            inactive();
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
 
                CogWheel[] adjoinCw = filterCogWheelsByCogActionType(cogWheels, CogAction.ADJOIN);
                foreach (CogWheel cw in adjoinCw)
                {
                    Debug.Log(name + "가 " + cw.name + "에게 givePower");

                    givePower(cw);
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
