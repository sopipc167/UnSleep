using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickCharacter : StoryInteract
{
    public Button button;
    private bool isClick = false;

    public override bool IsCompelete()
    {
        return isClick;
    }

    public void OnclickCharacter()
    {
        isClick = true;
        gameObject.SetActive(false);
    }
}
