using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]

public class WarpHole : MonoBehaviour
{
    public Transform destination;

    private LakeBall ball = null;
    private Vector3 originalScale;
    private float lineVelocity;
    private float rotatationVelocity;
    readonly WaitForSeconds delay = new WaitForSeconds(0.01f);

    private void Awake()
    {
        transform.localRotation = Quaternion.FromToRotation(Vector3.up, transform.position - Vector3.zero);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (ball == null)
            {
                ball = collision.gameObject.GetComponent<LakeBall>();
            }

            if (ball.canWarp)
            {
                ball.canWarp = false;
                originalScale = ball.transform.localScale;
                lineVelocity = ball.velocity;
                rotatationVelocity = ball.ballManager.velocity;
                StartCoroutine(HoleCoroutine());
            }
        }
    }

    private IEnumerator HoleCoroutine()
    {
        float scale = originalScale.x * 0.01f;
        while (ball.transform.localScale.x > scale)
        {
            ball.transform.localScale *= 0.9f;
            ball.velocity *= 0.9f;
            ball.ballManager.velocity *= 0.9f;
            yield return delay;

            //블랙홀 스프라이트 이후 넘어가지 않게
            if (Vector3.Distance(transform.position, ball.transform.position) < 0.2f)
            {
                break;
            }
        }

        ball.ballManager.transform.rotation = destination.rotation;
        ball.transform.position = destination.position;

        //부동소수점 오류 최소화
        ball.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        ball.velocity = lineVelocity * 0.1f;
        ball.ballManager.velocity = rotatationVelocity * 0.1f;

        while (ball.transform.localScale.x < originalScale.x)
        {
            ball.transform.localScale *= 1.1f;
            ball.velocity *= 1.1f;
            ball.ballManager.velocity *= 1.1f;
            yield return delay;
        }

        ball.canWarp = true;
        ball.transform.localScale = originalScale;
        ball.velocity = lineVelocity;
        ball.ballManager.velocity = rotatationVelocity;
    }
}
