using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WCogWheel : MonoBehaviour, CogWheel
{
    public CogWheelInfo info;
    protected CogWheelSpriteManager spriteManager;
    private CogWheel overlapChild = null;

    private Vector3 clickOffset;
    private const float lerpSpeed = 10f;


    private void Start()
    {
        spriteManager = GetComponent<CogWheelSpriteManager>();
        info.radius = Vector2.Distance(transform.GetChild(0).position, transform.GetChild(1).position);
        actionUp(detect());
    }

    private void OnMouseDown()
    {
        clickOffset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        if (info.state == CogState.INACTIVE) info.state = CogState.IDLE;
    }

    private void OnMouseDrag()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - clickOffset;
        stop();
        actionDrag(detect());
     
    }

    private void OnMouseUp()
    {
        actionUp(detect());
    }

    void Update()
    {
        switch (info.state)
        {
            case CogState.ROTATE: 
                transform.Rotate(new Vector3(0f, 0f, info.speed * (int)info.rotation * 0.01f)); 
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

        CogRotation criteria = adjoinWheels[0].getCogWheelInfo().rotation;

        foreach (CogWheel cw in adjoinWheels)
        {
            CogRotation rotation = cw.getCogWheelInfo().rotation;
            if (rotation == CogRotation.IDLE) continue;
            if (rotation != criteria) return false;
        }

        return true;
    }

    private CogWheel[] filterCogWheelsByCogActionType(CogWheel[] cogWheels, CogAction action)
    {
        return cogWheels.Filter(cw => CogWheelUtil.getCogAction(this, cw) == action);
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

        CogAction nearestCogAction = CogWheelUtil.getCogAction(this, cogWheels[0]);

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


        switch (CogWheelUtil.getCogAction(this, cogWheels[0]))
        {
            case CogAction.ADJOIN:
                if (info.state == CogState.IDLE)
                {
                    getPower(cogWheels[0]);
                }
 
                CogWheel[] adjoinCw = filterCogWheelsByCogActionType(cogWheels, CogAction.ADJOIN);
                foreach (CogWheel cw in adjoinCw)
                {
                    givePower(cw);
                }
                break;
            case CogAction.FAR:
                break;
            case CogAction.RESTRICT:
                inactive();
                break;
            case CogAction.OVERLAP:
                overlap(cogWheels[0] as WCogWheel);
                CogWheel[] overlapAdjoinCw = filterCogWheelsByCogActionType(cogWheels, CogAction.ADJOIN);
                foreach (CogWheel cw in overlapAdjoinCw) givePower(cw);
                break;
        }


        CogWheel[] farCw = filterCogWheelsByCogActionType(cogWheels, CogAction.FAR);
        foreach (CogWheel cw in farCw) if (cw.isAlone()) cw.stop();

    }

    public void givePower(CogWheel other)
    {
        CogWheelInfo otherInfo = other.getCogWheelInfo();
        if (info.state == CogState.INACTIVE || info.state == CogState.IDLE || 
            otherInfo.state == CogState.ROTATE || otherInfo.state == CogState.OVERLAP) return;

        switch (info.rotation)
        {
            case CogRotation.CLOCKWISE:
                other.receive(CogRotation.COUNTERCLOCKWISE, (info.speed * info.size) / otherInfo.size, info.level, transform.position.z);
                break;
            case CogRotation.COUNTERCLOCKWISE:
                other.receive(CogRotation.CLOCKWISE, (info.speed * info.size) / otherInfo.size, info.level, transform.position.z);
                break;
        }
    }

    public void getPower(CogWheel other)
    {
        CogWheelInfo otherInfo = other.getCogWheelInfo();
        if (info.state == CogState.ROTATE || info.state == CogState.OVERLAP || 
            otherInfo.state == CogState.INACTIVE || otherInfo.state == CogState.IDLE) return;

        switch (otherInfo.rotation)
        {
            case CogRotation.CLOCKWISE:
                receive(CogRotation.COUNTERCLOCKWISE, (otherInfo.speed * otherInfo.size) / info.size, otherInfo.level, other.getPosition().z);
                break;
            case CogRotation.COUNTERCLOCKWISE:
                receive(CogRotation.CLOCKWISE, (otherInfo.speed * otherInfo.size) / info.size, otherInfo.level, other.getPosition().z);
                break;
        }
    }


    public void receive(CogRotation r, float s, int l, float z)
    {
        info.updateInfo(CogState.ROTATE, r, s, l);
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }

    public void overlap(WCogWheel other) // 내가 other 위에 꽂힌다
    {
        CogWheelInfo otherInfo = other.getCogWheelInfo();
        if (info.state == CogState.ROTATE || info.state == CogState.OVERLAP ||
            otherInfo.state == CogState.INACTIVE || otherInfo.state == CogState.IDLE) return;

        Vector3 parentPosition = other.getPosition();
        other.setOverlapChild(this);
        transform.position = parentPosition;
        receive(otherInfo.rotation, otherInfo.speed, otherInfo.level + 1, parentPosition.z + (info.size <= otherInfo.size ? -1f : 1f));
    }

    public void setOverlapChild(CogWheel child)
    {
        overlapChild = child;
    }

    public void stop()
    {
        if (info.state == CogState.INACTIVE) return;

        changeState(CogState.IDLE);
    }

    public void idle()
    {
        changeState(CogState.IDLE);
    }

    public void inactive()
    {
        if (info.state == CogState.INACTIVE) return;
        changeState(CogState.INACTIVE);
    }

    public CogWheel[] detect()
    {
        List<CogWheel> cogWheels = new List<CogWheel>();
        cogWheels.AddRange(FindObjectsOfType<BCogWheel>());
        cogWheels.AddRange(FindObjectsOfType<WCogWheel>());
        return CogWheelUtil.sortByDistance(this, cogWheels.ToArray().Filter(cw => cw.getCogWheelInfo().state != CogState.INACTIVE));
    }
    public bool isAlone()
    {
        foreach (CogWheel cw in detect())
        {
            if (CogWheelUtil.getCogAction(this, cw) != CogAction.FAR)
                return false;
        }
        return true;

    }

    public void changeState(CogState newState)
    {
        switch (newState)
        {
            case CogState.IDLE:
                info.updateInfo(newState, CogRotation.IDLE, 0f, 0);
                spriteManager.setSprite(0);
                spriteManager.setColor(Color.white);       
                break;
            case CogState.INACTIVE:
                info.updateInfo(newState, CogRotation.IDLE, 0f, 0);
                spriteManager.setSprite(0);
                spriteManager.setColor(Color.gray);
                break;
            case CogState.ROTATE:
                spriteManager.setSprite(info.level);
                spriteManager.setColor(Color.white);
                break;
        }

    }
    public bool hasOverlap()
    {
        return overlapChild != null;
    }

    public Vector3 getPosition()
    {
        return transform.position;
    }


    public CogWheelInfo getCogWheelInfo()
    {
        return info;
    }
}
