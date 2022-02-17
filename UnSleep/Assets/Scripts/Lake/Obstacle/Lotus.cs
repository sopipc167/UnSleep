using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lotus : MonoBehaviour
{
    private LotusManager lotusManager;
    private bool isFirst = true;

    private void Start()
    {
        lotusManager = transform.parent.GetComponent<LotusManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isFirst)
            {
                isFirst = false;
                lotusManager.canMove = true;

                var ball = collision.gameObject.GetComponent<LakeBall>();
                lotusManager.velocity = ball.ballManager.velocity;
                if (ball.ballManager.isRight)
                {
                    lotusManager.isRight = true;
                }
                else if (ball.ballManager.isLeft)
                {
                    lotusManager.isLeft = true;
                }
            }
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            gameObject.tag = "Wall";
            lotusManager.isRight = false;
            lotusManager.isLeft = false;
            lotusManager.velocity = 0f;
            lotusManager.canMove = false;
        }
    }
}