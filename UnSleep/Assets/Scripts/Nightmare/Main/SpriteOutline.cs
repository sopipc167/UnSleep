using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour
{
    public Color color = Color.white;

    [Range(0, 16)]
    public int outlineSize = 4;

    public SpriteRenderer spriteRenderer;
    public bool isOut = false;


    void Update()
    {
        if (isOut)
            UpdateOutline(true);
    }


    private void OnMouseEnter()
    {
        Debug.Log("Enter");
        //UpdateOutline(true);
    }

    private void OnMouseOver()
    {
        Debug.Log("Over");
    }

    private void OnMouseExit()
    {
        Debug.Log("Exit");
        //UpdateOutline(false);
    }


    void UpdateOutline(bool outline)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", color);
        mpb.SetFloat("_OutlineSize", outlineSize);
        spriteRenderer.SetPropertyBlock(mpb);
    }
}