using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowLakeMovement : MonoBehaviour
{
    public LakeBall ball;
    public Transform ballManager;
    public Image[] buttons;
    public Color changeColor;

    protected Vector3 defaultBallPos;
    protected Quaternion defaultBallRot;

    protected Vector3 defaultBallManagerPos;
    protected Quaternion defaultBallManagerRot;

    protected readonly WaitForSeconds delay = new WaitForSeconds(0.1f);

    protected virtual void Awake()
    {
        defaultBallPos = ball.transform.position;
        defaultBallRot = ball.transform.rotation;
        defaultBallManagerPos = ballManager.position;
        defaultBallManagerRot = ballManager.rotation;
    }

    protected void OnEnable()
    {
        StartCoroutine(MoveTargetCoroutine());
    }

    protected void OnDisable()
    {
        StopAllCoroutines();
    }

    protected virtual IEnumerator MoveTargetCoroutine()
    {
        yield return delay;
        while (true)
        {
            // 위치 조정
            ballManager.SetPositionAndRotation(defaultBallManagerPos, defaultBallManagerRot);
            ball.transform.SetPositionAndRotation(defaultBallPos, defaultBallRot);

            ball.BallUIOn();

            Color tempColor;
            char buttonType;

            foreach (var item in buttons)
            {
                // 버튼 애니메이션
                item.color = Color.white;
                tempColor = Color.white - changeColor;
                tempColor /= 0.7f;

                while (item.color.b > changeColor.b)
                {
                    item.color -= tempColor * Time.deltaTime;
                    yield return null;
                }
                item.color = changeColor;

                while (item.color.b < 0.99f)
                {
                    item.color += tempColor * Time.deltaTime;
                    yield return null;
                }
                item.color = Color.white;

                // 움직임
                buttonType = item.gameObject.name[2];
                switch (buttonType)
                {
                    case 'g': ball.OnClickRightButton(); break;
                    case 'f': ball.OnClickLeftButton(); break;
                    case 'o': ball.OnClickFrontButton(); break;
                    default: ball.OnClickRearButton(); break;
                }

                yield return delay;
                yield return new WaitUntil(() => ball.rightButton.activeSelf);
            }

        }
    }
}
