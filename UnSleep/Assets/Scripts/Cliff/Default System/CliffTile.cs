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
    internal static float ALPHA_VALUE = 0.4f;

    public CliffType type;

    internal CliffTile parentTile = null;
    internal bool canRevert = false;    // == destroyed
    internal bool isDestroying = false;
    internal bool isReverting = false;
    internal bool startAniFlag = true;


    private SpriteRenderer spriteRenderer;
    private ParticleSystem particle;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        particle = transform.parent.GetChild(0).GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        if (startAniFlag)
        {
            StartCoroutine(StartRotationCoroutine());
        }
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

    public void ChangeAlpha(float value)
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, value);
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
        while (spriteColor.a > ALPHA_VALUE)
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
        particle.IsAlive(false);

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