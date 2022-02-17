using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Map
{
    public int route;
    public bool isTalk;
    public int talk_id;
    public bool isAudio;
    public string sound_position;
    public int sound_index;
    public float volume;
    public int sound_index_2;
    public float volume_2;
    public bool isObject;
    public int object_index;

    public bool isSave;
    public string mapcode;
}


[System.Serializable]
public class MapArray
{
    public Map[] Col;
}
