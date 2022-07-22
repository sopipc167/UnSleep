using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ChangeLayout : MonoBehaviour
{

    public RectTransform NAME;
    public RectTransform LINE;
    public RectTransform speaker1;
    public RectTransform speaker2;


    private int LastLayout;


    public void LayoutChange(int layoutnum)
    {
        if (LastLayout == layoutnum) //바꾸려는 레이아웃이 현재 레이아웃과 같다면 그만
            return;

        if (layoutnum == 1)
        {
            LINE.DOAnchorPos(new Vector2(0, 0), 1);
            NAME.DOAnchorPos(new Vector2(0, -12), 1);
            speaker1.DOAnchorPos(new Vector2(-460, -45), 1);
            speaker2.DOAnchorPos(new Vector2(460, -45), 1);
            LastLayout = 1;
        }
        else if (layoutnum == 2)
        {
            LINE.DOAnchorPos(new Vector2(0, -581), 1);
            NAME.DOAnchorPos(new Vector2(0, -593), 1);
            speaker1.DOAnchorPos(new Vector2(-460, -1043), 1);
            speaker2.DOAnchorPos(new Vector2(460, -1043), 1);
            LastLayout = 2;
        }
        else if (layoutnum == 3)
        {
            LINE.DOAnchorPos(new Vector2(0, -581), 1);
            NAME.DOAnchorPos(new Vector2(0, -593), 1);
            speaker1.DOAnchorPos(new Vector2(-460, -1043), 1);
            speaker2.DOAnchorPos(new Vector2(460, -1043), 1);
            LastLayout = 3;
        }
        else if (layoutnum == 4)
        {
            LINE.DOAnchorPos(new Vector2(0, -100), 1);
            NAME.DOAnchorPos(new Vector2(0, -200), 1);
            speaker1.DOAnchorPos(new Vector2(-460, -1043), 1);
            speaker2.DOAnchorPos(new Vector2(460, -1043), 1);
            LastLayout = 4;
        }
        else if (layoutnum == 7)
        {
            LINE.DOAnchorPos(new Vector2(0, -581), 1);
            NAME.DOAnchorPos(new Vector2(0, -593), 1);
            speaker1.DOAnchorPos(new Vector2(-460, -1043), 1);
            speaker2.DOAnchorPos(new Vector2(460, -1043), 1);
            LastLayout = 7;
        }
    }

}
