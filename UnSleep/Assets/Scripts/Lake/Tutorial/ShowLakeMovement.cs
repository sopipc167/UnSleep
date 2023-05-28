using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowLakeMovement : MonoBehaviour
{
    public LakeBall ball;
    public BallManager ballManager;
    public Image[] buttons;
    public Color changeColor;
    public Color defaultColor;

    private Vector3 bPos;
    private Quaternion bRot;
    private Vector3 bScl;

    private Vector3 bmPos;
    private Quaternion bmRot;
    private Vector3 bmScl;

    protected readonly WaitForSeconds delay = new WaitForSeconds(0.5f);

    protected virtual void Start()
    {
        bPos = ball.transform.localPosition;
        bRot = ball.transform.localRotation;
        bScl = ball.transform.localScale;

        bmPos = ballManager.transform.localPosition;
        bmRot = ballManager.transform.localRotation;
        bmScl = ballManager.transform.localScale;
    }

    protected void OnEnable()
    {
        StartCoroutine(MoveTargetCoroutine());
    }

    protected void OnDisable()
    {
        StopAllCoroutines();
        ResetData();
    }

    protected virtual void ResetData()
    {
        ball.Stop();
        ballManager.Stop();
        ball.BallUIOff();

        foreach (var item in buttons)
        {
            item.color = defaultColor;
        }

        ball.transform.localPosition = bPos;
        ball.transform.localRotation = bRot;
        ball.transform.localScale = bScl;

        ballManager.transform.localPosition = bmPos;
        ballManager.transform.localRotation = bmRot;
        ballManager.transform.localScale = bmScl;
    }

    protected IEnumerator MoveTargetCoroutine()
    {
        yield return delay;
        while (true)
        {
            ResetData();
            ball.BallUIOn();

            Color tempColor;
            char buttonType;

            foreach (var item in buttons)
            {
                // 버튼 애니메이션
                item.color = defaultColor;
                tempColor = changeColor - defaultColor;
                tempColor /= 0.7f;

                while (changeColor.r - item.color.r > 0.01f)
                {
                    item.color += tempColor * Time.deltaTime;
                    yield return null;
                }
                item.color = changeColor;

                while (item.color.r - defaultColor.r > 0.01f)
                {
                    item.color -= tempColor * Time.deltaTime;
                    yield return null;
                }
                item.color = defaultColor;

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
