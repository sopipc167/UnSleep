using System.Collections.Generic;
using UnityEngine;

public class CogWheel : MonoBehaviour
{
    public CogState state;
    public CogRotation rotation;
    public float speed;
    public int size;
    public int level;

    protected float radius;
    protected CogWheelSpriteManager spriteManager;
    private CogWheel overlapChild = null;


    private const float offset = 0.12f;

    // Start is called before the first frame update
    void Start()
    {
        spriteManager = GetComponent<CogWheelSpriteManager>(); 
        radius = Vector2.Distance(transform.GetChild(0).position, transform.GetChild(1).position);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == CogState.ROTATE)
        {
            transform.Rotate(new Vector3(0f, 0f, speed * (int)rotation * 0.01f));
        }
    }



    public void givePower(CogWheel other)
    {
        if (state == CogState.INACTIVE || state == CogState.IDLE) return;
        if (other.state == CogState.ROTATE || other.state == CogState.OVERLAP) return;

        CogRotation giveRotation;
        if (rotation == CogRotation.CLOCKWISE)
        {
            giveRotation = CogRotation.COUNTERCLOCKWISE;
        }
        else if (rotation == CogRotation.COUNTERCLOCKWISE)
        {
            giveRotation = CogRotation.CLOCKWISE;
        }
        else
        {
            return;
        }

        other.receive(giveRotation, speed * ((float)size / (float)other.size), level, transform.position.z);
    }

    public virtual void getPower(CogWheel other)
    {
        if (state == CogState.ROTATE || state == CogState.OVERLAP) return;
        if (other.state == CogState.INACTIVE || other.state == CogState.IDLE) return;

        CogRotation getRotation;
        if (other.rotation == CogRotation.CLOCKWISE)
        {
            getRotation = CogRotation.COUNTERCLOCKWISE;
        }
        else if (other.rotation == CogRotation.COUNTERCLOCKWISE)
        {
            getRotation = CogRotation.CLOCKWISE;
        }
        else
        {
            return;
        }

        receive(getRotation, other.speed * ((float)size / (float)other.size), other.level, other.gameObject.transform.position.z);
    }

    public void overlap(CogWheel other) // 내가 other 위에 꽂힌다
    {
        if (state == CogState.ROTATE || state == CogState.OVERLAP) return;
        if (other.state == CogState.INACTIVE || other.state == CogState.IDLE) return;
        
        Vector3 parentPosition = other.gameObject.transform.position;
        other.setOverlapChild(this);
        transform.position = parentPosition;

        if (size <= other.size)
        {
            receive(other.rotation, other.speed, other.level + 1, parentPosition.z - 1f);
        } else
        {
            receive(other.rotation, other.speed, other.level + 1, parentPosition.z + 1f);
        }
    }

    public void receive(CogRotation r, float s, int l, float z)
    {
        // Debug.Log(string.Format("{0}: {1}로 {2}만큼 돌아간다", name, r, s));
        rotation = r;
        speed = s;
        level = l;
        changeState(CogState.ROTATE);
        transform.position = new Vector3(transform.position.x, transform.position.y, z);


    }

    public virtual void stop() // INACTIVE를 무시
    {
        if (state == CogState.INACTIVE) return;

        changeState(CogState.IDLE);
    }

    public void idle() // INACTIVE를 IDLE로 변경
    {
        changeState(CogState.IDLE);
    }

    public void inactive()
    {
        if (state == CogState.INACTIVE) return;
        changeState(CogState.INACTIVE);
    }

    protected CogWheel[] detect()
    {
        return sortByDistance(FindObjectsOfType<CogWheel>().Filter(cw => cw.state != CogState.INACTIVE));
    }

    public bool isAlone()
    {
        foreach (CogWheel cw in detect())
        {
            if (getCogAction(cw) != CogAction.FAR)
                return false;
        }
        return true;
    }

    private int compareDistance(CogWheel A, CogWheel B)
    {
        float distA = Vector2.Distance(transform.position, A.transform.position);
        float distB = Vector2.Distance(transform.position, B.transform.position);
        
        if (distA < distB) return -1;
        else if (distA == distB)
        {
            if (A.hasOverlap() || B.hasOverlap())
            {
                if (distA - radius - A.radius + offset < 0) return 1;
                if (distB - radius - B.radius + offset < 0) return -1;

                if (distA - radius - A.radius + offset < distB - radius - B.radius + offset) return -1;
                else if (distA - radius - A.radius + offset > distB - radius - B.radius + offset) return 1;
                else return 0;
            }
            else
                return 0;
        }
        else return 1;
    }


    protected CogAction getCogAction(CogWheel other)
    {
        float dist = Vector2.Distance(transform.position, other.transform.position);
        float criteria = radius + other.radius;

        if (dist <= offset) return CogAction.OVERLAP;
        else if (dist < criteria - offset) return CogAction.RESTRICT;
        else if (dist <= criteria + offset) return CogAction.ADJOIN;
        else return CogAction.FAR;
    }

    private CogWheel[] sortByDistance(CogWheel[] cogs)
    {
        List<CogWheel> list = new List<CogWheel>(cogs);
        list.Remove(this); // 나 자신은 빼고
        list.Sort((a, b) => compareDistance(a, b));
        return list.ToArray();
    }

    public void setOverlapChild(CogWheel child)
    {
        overlapChild = child;
    }

    public bool hasOverlap()
    {
        return overlapChild != null;
    }

    public void changeState(CogState newState)
    {
        state = newState;
        switch (newState)
        {
            case CogState.IDLE:
                rotation = CogRotation.IDLE;
                speed = 0f;
                level = 0;
                if (spriteManager != null)
                {
                    spriteManager.setSprite(0);
                    spriteManager.setColor(Color.white);
                }
                break;
            case CogState.INACTIVE:
                if (spriteManager != null)
                {
                    spriteManager.setSprite(0);
                    spriteManager.setColor(Color.gray);
                }
                speed = 0f;
                rotation = CogRotation.IDLE;
                level = 0;
                break;
            case CogState.ROTATE:
                if (spriteManager != null)
                {
                    spriteManager.setSprite(level);
                    spriteManager.setColor(Color.white);
                }
                break;


        }
    }
}

public enum CogRotation
{
    CLOCKWISE = -1, IDLE = 0, COUNTERCLOCKWISE = 1
}

public enum CogState
{
    ROTATE, INACTIVE, IDLE, OVERLAP
}

public enum CogAction
{
    OVERLAP, RESTRICT, ADJOIN, FAR
}