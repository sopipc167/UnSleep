using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{

    public GameObject Triggers;
    private Spawn spawn;

    void Start()
    {
        Triggers = GameObject.FindWithTag("MapTrigger");
        spawn = Triggers.GetComponent<Spawn>();

    }

    

    public void SetPlayerPos(string place)
    {
        Vector3 pos = spawn.GetTriggerPos(place);
        this.transform.position = pos;

    }
}
