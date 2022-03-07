using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowLakeDuck : ShowLakeMovement
{
    public SpriteRenderer parentDuck;
    public SpriteRenderer[] children;

    protected override IEnumerator MoveTargetCoroutine()
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

            parentDuck.gameObject.SetActive(true);
            parentDuck.color = Color.white;
            ball.duckCnt = 0;
            foreach (var item in children)
            {
                item.gameObject.SetActive(true);
                item.color = Color.white;
            }
        }
    }
}
