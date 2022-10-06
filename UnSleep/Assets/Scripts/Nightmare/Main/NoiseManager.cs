using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseManager : MonoBehaviour
{
    static public NoiseManager instance;
    public bool isEndOpenning;
    public TextManager TM;

    public int[] diaGroup;

    void Start()
    {
        instance = this;
        Dialogue_Proceeder.instance.UpdateCurrentEpiID(6);
        Dialogue_Proceeder.instance.UpdateCurrentDiaID(2001);
    }


    void Update()
    {
        if (isEndOpenning)
        {
            isEndOpenning = false;
            StartDia(diaGroup[0]);
        }


    }

    public void StartDia(int diaID)
    {
        int[] conditions = TM.ReturnDiaConditions(diaID);

        if (Dialogue_Proceeder.instance.Satisfy_Condition(conditions))
        {
            Dialogue_Proceeder.instance.UpdateCurrentDiaID(diaID);
            TM.SetDiaInMap();
            TM.Increasediaindex = true;

            return;
        }
    }
}
