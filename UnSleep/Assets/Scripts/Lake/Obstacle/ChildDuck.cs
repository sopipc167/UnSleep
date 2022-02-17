using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildDuck : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<LakeBall>().duckCnt += 1;
            StartCoroutine(DestroyCoroutine());
        }
    }

    private IEnumerator DestroyCoroutine()
    {
        Color AlphaColor = spriteRenderer.color;
        while (AlphaColor.a > 0f)
        {
            AlphaColor.a -= 0.01f;
            spriteRenderer.color = AlphaColor;
            yield return null;
        }
        Destroy(gameObject);
    }
}