using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleBtn2 : StoryInteract
{
    private bool[] conditions = { false, false, false };
    public Button[] btns;

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

    public override bool IsCompelete()
    {
        return conditions[0] && conditions[1] && conditions[2];
    }
}
