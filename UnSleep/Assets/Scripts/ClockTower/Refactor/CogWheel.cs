using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogWheel : MonoBehaviour
{
    public CogState state;
    public CogRotation rotation;
    public float speed;
    public int size;

    private float radius;
   

    private const float offset = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        radius = Vector2.Distance(transform.GetChild(0).position, transform.GetChild(1).position);
        detect();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == CogState.ROTATE)
        {
            transform.Rotate(new Vector3(0f, 0f, speed * (int)rotation * 0.01f));
        }
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
                    break;
                case CogAction.RESTRICT:
                    break;
                case CogAction.OVERLAP:
                    break;
            }
        }
    }

    public void givePower(CogWheel other)
    {
        if (state == CogState.INACTIVE || state == CogState.IDLE) return;

        CogRotation giveRotation;
        if (rotation == CogRotation.CLOCKWISE)
        {
            giveRotation = CogRotation.COUNTERCLOCKWISE;
        } else if (rotation == CogRotation.COUNTERCLOCKWISE)
        {
            giveRotation = CogRotation.CLOCKWISE;
        } else
        {
            return;
        }

        other.receive(giveRotation, speed * ((float)size / (float)other.size));
    }

    public void getPower(CogWheel other)
    {
        if (state == CogState.ROTATE || state == CogState.OVERLAP) return;

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

    private CogWheel[] sortByDistance(CogWheel[] cogs)
    {
        List<CogWheel> list = new List<CogWheel>(cogs);
        list.Remove(this); // 나 자신은 빼고
        list.Sort((a, b) => compareDistance(a, b));
        return list.ToArray();
    }

    private int compareDistance(CogWheel A, CogWheel B)
    {
        float distA = Vector3.Distance(transform.position, A.transform.position);
        float distB = Vector3.Distance(transform.position, B.transform.position);

        if (distA < distB) return -1;
        else if (distA == distB) return 0;
        else return 1;
    }

    private CogAction getCogAction(CogWheel other)
    {
        float dist = Vector3.Distance(transform.position, other.transform.position);
        Debug.Log(dist);
        float criteria = radius + other.radius;

        if (dist <= offset) return CogAction.OVERLAP;
        else if (dist < criteria - offset) return CogAction.RESTRICT;
        else if (dist <= criteria + offset) return CogAction.ADJOIN;
        else return CogAction.FAR;
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