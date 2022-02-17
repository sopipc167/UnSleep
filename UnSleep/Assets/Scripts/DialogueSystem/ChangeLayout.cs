using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ChangeLayout : MonoBehaviour
{
    //Animator Dia_animator;

    public RectTransform NAME;
    public RectTransform LINE;
    public RectTransform speaker1;
    public RectTransform speaker2;





    void Start()
    {
        //Dia_animator = GetComponent<Animator>();

    }

    /*
    // Update is called once per frame
    void Update()
    {
        //Dia_animator.SetInteger("LayoutChange", LayoutChangeint);

    }

     */

    public void LayoutChange(int layoutnum)
    {
        if (layoutnum == 1)
        {
            LINE.DOAnchorPos(new Vector2(0, 0), 1);
            NAME.DOAnchorPos(new Vector2(0, -12), 1);
            speaker1.DOAnchorPos(new Vector2(-460, -45), 1);
            speaker2.DOAnchorPos(new Vector2(460, -45), 1);
        }
        else if (layoutnum == 2 || layoutnum ==3)
        {
            LINE.DOAnchorPos(new Vector2(0, -581), 1);
            NAME.DOAnchorPos(new Vector2(0, -593), 1);
            speaker1.DOAnchorPos(new Vector2(-460, -1043), 1);
            speaker2.DOAnchorPos(new Vector2(460, -1043), 1);
        }
        else if (layoutnum == 4)
        {
            LINE.DOAnchorPos(new Vector2(0, -240), 1);
            NAME.DOAnchorPos(new Vector2(-500, -345), 1);
            speaker1.DOAnchorPos(new Vector2(-460, -1043), 1);
            speaker2.DOAnchorPos(new Vector2(460, -1043), 1);
        }
    }

}
