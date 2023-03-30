using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogWheelUtil
{
    private const float offset = 0.12f;

    public static CogAction getCogAction(CogWheel root, CogWheel other)
    {
        float dist = Vector2.Distance(root.getPosition(), other.getPosition());
        float criteria = root.getCogWheelInfo().radius + other.getCogWheelInfo().radius;

        if (dist <= offset) return CogAction.OVERLAP;
        else if (dist < criteria - offset) return CogAction.RESTRICT;
        else if (dist <= criteria + offset) return CogAction.ADJOIN;
        else return CogAction.FAR;
    }

    public static CogWheel[] sortByDistance(CogWheel root, CogWheel[] cogs)
    {
        List<CogWheel> list = new List<CogWheel>(cogs);
        list.Remove(root); // 나 자신은 빼고
        list.Sort((a, b) => compareDistance(root, a, b));
        return list.ToArray();
    }

    public static int compareDistance(CogWheel root, CogWheel A, CogWheel B)
    {
        float distA = Vector2.Distance(root.getPosition(), A.getPosition());
        float distB = Vector2.Distance(root.getPosition(), B.getPosition());
        float rootRadius = root.getCogWheelInfo().radius;

        if (distA < distB) return -1;
        else if (distA == distB)
        {
            if (A.hasOverlap() || B.hasOverlap())
            {
                if (distA - rootRadius - A.getCogWheelInfo().radius + offset < 0) return 1;
                if (distB - rootRadius - B.getCogWheelInfo().radius + offset < 0) return -1;

                if (distA - rootRadius - A.getCogWheelInfo().radius + offset 
                    < distB - rootRadius - B.getCogWheelInfo().radius + offset) return -1;
                else if (distA - rootRadius - A.getCogWheelInfo().radius + offset 
                    > distB - rootRadius - B.getCogWheelInfo().radius + offset) return 1;
                else return 0;
            }
            else
                return 0;
        }
        else return 1;
    }

}
