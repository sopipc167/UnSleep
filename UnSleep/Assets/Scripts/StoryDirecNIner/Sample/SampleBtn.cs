using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SampleBtn : MonoBehaviour
{
    private bool[] conditions;
    public Button[] btns;
    
    // Start is called before the first frame update
    void Start()
    {
        conditions = new bool[] { false, false, false };
    }

    // Update is called once per frame
    void Update()
    {
        if (conditions[0]&& conditions[1]&& conditions[2])
        {
            GetComponent<Complete_DnI>().Complete_Direc_and_Inter();
        }
    }

    public void btn0()
    {
        conditions[0] = true;
        btns[0].interactable = false;
    }

    public void btn1()
    {
        conditions[1] = true;
        btns[1].interactable = false;
    }

    public void btn2()
    {
        conditions[2] = true;
        btns[2].interactable = false;
    }
}
