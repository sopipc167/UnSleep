using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageButton : StoryInteract
{
    public GameObject phoneObj;
    public GameObject[] textObjs;

    private int curIdx = 0;
    private bool result = false;

    public override bool IsCompelete()
    {
        return result;
    }

    private void Update()
    {
        if (!phoneObj.activeSelf) return;

        if (Input.GetMouseButtonDown(0))
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
