using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public enum CliffShapeType { None, Circle, Triangle, Square, Pentagon }

[SerializeField]
public enum CliffColorType { None, Red, Yellow, Green, Blue }

[System.Serializable]
public struct CliffType
{
    public CliffShapeType shape;
    public CliffColorType color;
}

public class CliffTile : MonoBehaviour
{
    public CliffType type;

    internal CliffTile parentTile = null;

    internal bool canRevert = false;    //== destroyed
    internal bool isDestroying = false;
    internal bool isReverting = false;

    private SpriteRenderer spriteRenderer;
    private ParticleSystem particle;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        particle = transform.parent.GetChild(0).GetComponent<ParticleSystem>();
        StartCoroutine(StartRotationCoroutine());
    }

    private void OnMouseEnter()
    {
        if (!canRevert)
        {
            CliffManager.shouldCheckStatus = true;
        }
    }

    public void DestroyShape()
    {
        canRevert = true;
        StopAllCoroutines();
        StartCoroutine(DestroyShapeCoroutine());
    }

    public void RevertShape()
    {
        canRevert = false;
        StopAllCoroutines();
        StartCoroutine(RevertShapeCoroutine());
    }

    public void ClearPhase()
    {
        canRevert = false;
        StopAllCoroutines();
        StartCoroutine(ClearShapeCouroutine());
    }

    public void ChangeColor(Color color)
    {
        spriteRenderer.color = color;
    }

    private IEnumerator StartRotationCoroutine()
    {
        particle.Stop();
        Vector3 tmpVec = new Vector3(90f, 0f, 0f);
        transform.rotation = Quaternion.Euler(tmpVec);
        yield return new WaitForSeconds(Random.Range(0f, 1.2f));

        while (transform.rotation.x > 0f)
        {
            transform.rotation = Quaternion.Euler(tmpVec);
            tmpVec.x -= 180f * Time.deltaTime;
            yield return null;
        }
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        particle.Play();
    }

    private IEnumerator DestroyShapeCoroutine()
    {
        isDestroying = true;
        particle.Stop();

        Color spriteColor = spriteRenderer.color;
        while (spriteColor.a > 0.4f)
        {
            spriteColor.a -= 0.6f * Time.deltaTime;
            spriteRenderer.color = spriteColor;
            yield return null;
        }

        isDestroying = false;
    }

    private IEnumerator RevertShapeCoroutine()
    {
        isReverting = true;
        particle.Play();

        Color spriteColor = spriteRenderer.color;
        while (spriteColor.a < 0.99f)
        {
            spriteColor.a += 0.6f * Time.deltaTime;
            spriteColor.r += 0.4f * Time.deltaTime;
            spriteColor.g += 0.4f * Time.deltaTime;
            spriteColor.b += 0.4f * Time.deltaTime;
            spriteRenderer.color = spriteColor;
            yield return null;
        }
        spriteColor.a = 1f;
        spriteRenderer.color = Color.white;
        isReverting = false;
    }

    private IEnumerator ClearShapeCouroutine()
    {
        isDestroying = true;
        particle.Clear();

        Color spriteColor = Color.white;
        Vector3 scale = transform.localScale;
        while (spriteColor.a > 0.01f)
        {
            scale.x += 0.2f * Time.deltaTime;
            scale.y += 0.2f * Time.deltaTime;
            transform.localScale = scale;

            spriteColor.a -= 1f * Time.deltaTime;
            spriteRenderer.color = spriteColor;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}