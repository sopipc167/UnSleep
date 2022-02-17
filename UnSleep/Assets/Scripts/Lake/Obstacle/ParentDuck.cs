using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentDuck : MonoBehaviour
{
    [Header("공 오브젝트")]
    public LakeBall ball;

    [Header("자식오리의 총 마릿수")]
    public int childrenCnt;

    private SpriteRenderer spriteRenderer;
    private bool isChange = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (!isChange && ball.duckCnt == childrenCnt)
        {
            isChange = true;
            gameObject.tag = "Untagged";
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isChange)
            {
                StartCoroutine(DestroyCoroutine());
            }
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
