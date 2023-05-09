using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildDuck : MonoBehaviour
{
    [Header("소리")]
    public AudioClip sound;

    private SpriteRenderer spriteRenderer;
    private LakeBall ball;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (ball == null)
            {
                ball = collision.gameObject.GetComponent<LakeBall>();
            }

            ball.duckCnt += 1;
            SoundManager.Instance.PlaySE(sound);
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
        gameObject.SetActive(false);
    }
}