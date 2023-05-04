using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour
{
    public Color color = Color.white;

    public int outlineSize = 4;
    public bool isStop;

    public bool isClick;
    public static SpriteOutline instance;


    private void Start()
    {
        instance = this;
    }

    public void UpdateOutline(bool outline, SpriteRenderer spriteRenderer)
    {
        spriteRenderer.enabled = outline;
    }
}