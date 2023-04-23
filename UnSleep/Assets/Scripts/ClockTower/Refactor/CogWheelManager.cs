using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogWheelManager : MonoBehaviour
{
    private List<CogWheel> cogWheels = new List<CogWheel>();
    // Start is called before the first frame update
    void Start()
    {
        cogWheels.AddRange(FindObjectsOfType<WCogWheel>());
        cogWheels.AddRange(FindObjectsOfType<BCogWheel>());
    }

    public void resetAll()
    {
        cogWheels.ForEach(cog => cog.reset());
    }
}
