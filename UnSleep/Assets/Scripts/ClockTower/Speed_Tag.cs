using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speed_Tag : MonoBehaviour
{
    public GameObject Target_Gear;
    public Text Speedtext;
    public bool Static_Check;
    private float speed;

    private void Start()
    {
        Speedtext = GetComponent<Text>();
    }
    void Update()
    {
        speed = Target_Gear.GetComponent<Gear>().rotate_speed;
        if (speed <= 0f)
            Speedtext.text = "";
        else
        {
            if (speed%1f==0)
                Speedtext.text = speed.ToString();
            else
              Speedtext.text = speed.ToString("N2");

        }
    }


}
