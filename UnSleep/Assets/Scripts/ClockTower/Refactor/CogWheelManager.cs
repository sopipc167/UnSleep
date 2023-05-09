using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogWheelManager : MonoBehaviour
{
    private List<CogWheel> cogWheels = new List<CogWheel>();
    public BCogWheel startCogWheel;

    void Start()
    {
        cogWheels.AddRange(FindObjectsOfType<WCogWheel>());
        BCogWheel[] bCogWheels = FindObjectsOfType<BCogWheel>();
        cogWheels.AddRange(bCogWheels);
        startCogWheel = bCogWheels.Filter(cw => cw.bInfo.type == BCogWheelType.START)[0];
    }

    public void resetAll()
    {
        cogWheels.ForEach(cog => cog.reset());
        startCogWheel.activate();
    }
  
    
}
