using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    public GameObject[] TriggerPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        GenerateTrigger();
    }
       
    public void GenerateTrigger()
    {
        GameObject tri = Instantiate(TriggerPrefabs[Dialogue_Proceeder.instance.CurrentEpiID]);
        tri.transform.position = new Vector3(0f, 0f, 0f);
    }

}
