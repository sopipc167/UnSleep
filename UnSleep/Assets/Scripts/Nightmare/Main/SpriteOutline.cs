using UnityEngine;


[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour
{
    public Color color = Color.white;

    [Range(0, 16)]
    public int outlineSize = 4;

    private SpriteRenderer spriteRenderer;

    public void OnMouseEnter()
    {
        Debug.Log("Enter");
        spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateOutline(true);
    }

    public void OnMouseExit()
    {
        Debug.Log("Exit");
        UpdateOutline(false);
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