using System.Collections.Generic;
using UnityEngine;

public class CogWheel : MonoBehaviour
{
    public CogState state;
    public CogRotation rotation;
    public float speed;
    public int size;

    protected float radius;


    private const float offset = 0.15f;

    // Start is called before the first frame update
    void Start()
    {
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

        other.receive(giveRotation, speed * ((float)size / (float)other.size));
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

        receive(getRotation, other.speed * ((float)size / (float)other.size));
    }

    public void receive(CogRotation r, float s)
    {
        rotation = r;
        speed = s;
        state = CogState.ROTATE;
    }

    public virtual void stop()
    {
        rotation = CogRotation.IDLE;
        speed = 0f;
        state = CogState.IDLE;
    }

    protected CogWheel[] detect()
    {
        return sortByDistance(FindObjectsOfType<CogWheel>());
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
        else if (distA == distB) return 0;
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