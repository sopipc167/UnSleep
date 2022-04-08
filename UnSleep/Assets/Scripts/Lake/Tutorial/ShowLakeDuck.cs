using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowLakeDuck : ShowLakeMovement
{
    public ParentDuck parentDuck;
    public SpriteRenderer[] children;

    protected override void ResetData()
    {
        base.ResetData();
        parentDuck.gameObject.SetActive(true);
        ball.duckCnt = 0;
        parentDuck.ResetData();
        foreach (var item in children)
        {
            item.gameObject.SetActive(true);
            item.color = Color.white;
        }
    }
}
