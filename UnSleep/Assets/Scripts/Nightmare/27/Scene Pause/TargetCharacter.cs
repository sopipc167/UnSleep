using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCharacter : MonoBehaviour
{
    [Header("참조")]
    public SpriteRenderer spriteRenderer;

    public void ChangeSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}
