using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogWheelUtil
{
    private const float offset = 0.15f;

    public static CogAction getCogAction(CogWheel root, CogWheel other)
    {
        float centerDist = Vector2.Distance(root.getPosition(), other.getPosition());
        float edgeDist = centerDist - (root.getCogWheelInfo().radius + other.getCogWheelInfo().radius);

        if (centerDist <= offset) return CogAction.OVERLAP;
        else if (edgeDist < -offset) return CogAction.RESTRICT;
        else if (edgeDist <= offset) return CogAction.ADJOIN;
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
        float centerDistA = Vector2.Distance(root.getPosition(), A.getPosition());
        float centerDistB = Vector2.Distance(root.getPosition(), B.getPosition());
        float edgeDistA = centerDistA - (root.getCogWheelInfo().radius + A.getCogWheelInfo().radius);
        float edgeDistB = centerDistB - (root.getCogWheelInfo().radius + B.getCogWheelInfo().radius);

        if (isAdjust(edgeDistA) && isAdjust(edgeDistB))
        {
            return edgeDistA < edgeDistB ? -1 : 1;
        } else if (isAdjust(edgeDistA) && !isAdjust(edgeDistB))
        {
            return -1;
        }
        else if (!isAdjust(edgeDistA) && isAdjust(edgeDistB))
        {
            return 1;
        } else
        {
            return edgeDistA < edgeDistB ? -1 : 1;
        }

        /*
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
        */
    }

    public static void debugCogWheels(CogWheel[] cogWheels)
    {
        string str = "";
        foreach (CogWheel cw in cogWheels)
        {
            if (cw as WCogWheel)
            {
                str += (cw as WCogWheel).gameObject.name + " ";
            } else
            {
                str += (cw as BCogWheel).gameObject.name + " ";
            }
        }
    }

    private static bool isAdjust(float edgeDist)
    {
        return -offset <= edgeDist && edgeDist < offset;
    }
}
