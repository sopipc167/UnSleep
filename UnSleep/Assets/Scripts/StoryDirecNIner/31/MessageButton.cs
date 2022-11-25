using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageButton : StoryInteract
{
    public GameObject flagObj;
    public GameObject[] textObjs;

    private int curIdx = 0;
    private bool result = false;

    public override bool IsCompelete()
    {
        return result;
    }

    private void OnEnable()
    {
        result = false;
        foreach (var item in textObjs)
        {
            item.SetActive(false);
        }
    }

    private void Update()
    {
        if (flagObj != null && flagObj.activeSelf) return;

        if (result)
        {
            if (Dialogue_Proceeder.instance.CurrentDiaID == 3118 ||
                Dialogue_Proceeder.instance.CurrentDiaID == 5603 ||
                Dialogue_Proceeder.instance.CurrentDiaID == 8017)
            {
                gameObject.SetActive(false);
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            if (curIdx == textObjs.Length)
            {
                result = true;
                return;
            }

            textObjs[curIdx].SetActive(true);
            ++curIdx;
        }
    }
}
