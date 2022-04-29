using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickDomun24 : StoryInteract
{
    public Button button;
    private bool isClick = false;

    public override bool IsCompelete()
    {
        return isClick;
    }

    public void OnclickDomun()
    {
        isClick = true;
        gameObject.SetActive(false);
    }
}
