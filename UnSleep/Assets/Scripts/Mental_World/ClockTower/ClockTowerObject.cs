using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockTowerObject : MonoBehaviour
{
    public GameObject Gear1;
    public GameObject Gear2;
    public GameObject Gear3;
    public GameObject Gear4;
    public GameObject Gear5;
    public GameObject Gear6;
    public GameObject chu1;
    public GameObject chu2;
    public GameObject timeniddle;
    public GameObject minniddle;

    private float min = 5f / 60;
    private float sec = 5f;
    private bool isRand = false;

    public void OnEffect()
    {
        if (Dialogue_Proceeder.instance.CurrentEpiID == 4 ||
            Dialogue_Proceeder.instance.CurrentEpiID == 13)
        {
            isRand = true;
        }
        else if (Dialogue_Proceeder.instance.CurrentEpiID == 8 ||
            Dialogue_Proceeder.instance.CurrentEpiID == 14)
        {
            min = 60f / 60;
            sec = 60f;
        }
        else if (Dialogue_Proceeder.instance.CurrentEpiID == 9)
        {
            min = -10f / 60;
            sec = -10f;
        }
        else if (Dialogue_Proceeder.instance.CurrentEpiID == 12)
        {
            min = 1f / 60;
            sec = 1f;
        }

        // 5 14 19
        else
        {
            min = 5f / 60;
            sec = 5f;
        }
    }

    void Update()
    {
        Gear1.transform.Rotate(0, 0, 0.6f);
        Gear2.transform.Rotate(0, 0, -0.15f);
        Gear3.transform.Rotate(0, 0, 0.15f);
        Gear4.transform.Rotate(0, 0, -0.1f);
        Gear5.transform.Rotate(0, 0, 0.1f);
        Gear6.transform.Rotate(0, 0, -0.05f);

        if (isRand)
        {
            int rand = Random.Range(0, 10000);
            min = 5f / 60 + (rand < 5000 ? -0.02f : 0.02f);
            sec = 5f + (rand < 5000 ? -1f : 1f);
        }
        minniddle.transform.Rotate(0, 0, min * Time.deltaTime);
        timeniddle.transform.Rotate(0, 0, sec * Time.deltaTime);
    }
}
