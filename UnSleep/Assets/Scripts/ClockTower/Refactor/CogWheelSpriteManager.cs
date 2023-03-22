using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogWheelSpriteManager : MonoBehaviour
{
    public Sprite defaultSprite;
    public Sprite[] sprites;
    private SpriteRenderer spriteRender;
    private void Awake()
    {
        spriteRender = GetComponent<SpriteRenderer>();
    }

    public void setSprite(int level)
    {
        if (level > 0)
            spriteRender.sprite = sprites[level % (sprites.Length) - 1];
        else
            spriteRender.sprite = defaultSprite;
    }

    public void setColor(CogAction cogAction)
    {
        switch (cogAction)
        {
            case CogAction.FAR:
                spriteRender.color = Color.white;
                break;
            case CogAction.ADJOIN:
                spriteRender.color = Color.green;
                break;
            case CogAction.RESTRICT:
                spriteRender.color = Color.red;
                break;
            case CogAction.OVERLAP:
                spriteRender.color = Color.blue;
                break;
        }
        
    }

    public void setColor(Color c)
    {
        spriteRender.color = c;
    }
}
