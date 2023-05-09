using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentDuck : MonoBehaviour
{
    [Header("공 오브젝트")]
    public LakeBall ball;

    [Header("자식오리의 총 마릿수")]
    public int childrenCnt;

    [Header("소리")]
    public AudioClip clearSound;
    public AudioClip blockSound;

    private SpriteRenderer spriteRenderer;
    private bool isChange = false;

    public void ResetData()
    {
        isChange = false;
        spriteRenderer.color = Color.white;
        gameObject.tag = "Wall";
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isChange) return;

        if (ball.duckCnt == childrenCnt)
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
                SoundManager.Instance.PlaySE(clearSound);
                StartCoroutine(DestroyCoroutine());
            }
            else
            {
                SoundManager.Instance.PlaySE(blockSound);
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
        isChange = false;
        gameObject.tag = "Wall";
        gameObject.SetActive(false);
    }
}
