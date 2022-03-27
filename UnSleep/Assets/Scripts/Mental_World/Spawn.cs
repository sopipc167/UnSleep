using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class Spawn : MonoBehaviour
{
    private Dictionary<string, Vector3> TriggerPosDic;

    private void Awake()
    {
        SetTriggerPos();
    }

    public void SetTriggerPos()
    {
        int childCnt = this.transform.childCount;
        Transform[] triggers = new Transform[childCnt];

        TriggerPosDic = new Dictionary<string, Vector3>();
        for (int i=0; i < childCnt; i++)
        {
            triggers[i] = this.transform.GetChild(i);
            TriggerPosDic[triggers[i].name] = triggers[i].position;
            Debug.Log(triggers[i].name);
        }
        
    }

    public Vector3 GetTriggerPos(string place)
    {
        if (TriggerPosDic.ContainsKey(place))
            return TriggerPosDic[place];


        return TriggerPosDic["nullpos"];
    }
}
