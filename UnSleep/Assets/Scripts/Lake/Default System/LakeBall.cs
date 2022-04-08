using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LakeBall : LakeMovement
{
    [Header("임시데이터")]
    public GameObject temp;
    public GameObject ButtonCanvas;
    public GameObject MemoCanvas;

    [Header("공메니저 클래스")]
    public BallManager ballManager;

    [Header("공 UI 캔버스")]
    public GameObject rightButton;
    public GameObject leftButton;
    public GameObject FrontButton;
    public GameObject RearButton;
    private RectTransform ballButtonPos;

    [Header("수직방향 가속도"), Range(0.0005f, 0.005f)]
    public float lineAcceleration;

    [Header("수직방향 공 회전가속도"), Range(0.05f, 0.5f)]
    public float lineRotationAcceleration;

    internal bool canWarp = true;
    internal int duckCnt = 0;

    internal bool isFront = false;
    internal bool isRear = false;

    private bool isFrontStop = false;
    private bool isRearStop = false;

    private bool isLineMovement = false;
    private bool isRotationalMovement = false;

    readonly WaitForSeconds delay = new WaitForSeconds(0.01f);

    private void Awake()
    {
        ballButtonPos = rightButton.transform.parent.GetComponent<RectTransform>();
        InitAccel(lineAcceleration, lineRotationAcceleration);
        BallUIOff();
    }

    private void FixedUpdate()
    {
        if (isFront)
        {
            transform.localPosition += new Vector3(0, -velocity, 0);
            Accelerate();
            RotateBall(transform, true, false);
        }
        else if (isRear)
        {
            transform.localPosition += new Vector3(0, velocity, 0);
            Accelerate();
            RotateBall(transform, false, false);
        }
        else if (isFrontStop)
        {
            transform.localPosition += new Vector3(0, velocity, 0);
            SlowdownLine();
            RotateBall(transform, false, true);
        }
        else if (isRearStop)
        {
            transform.localPosition += new Vector3(0, -velocity, 0);
            SlowdownLine();
            RotateBall(transform, true, true);
        }
        else
        {
            velocity = 0f;
        }

        if (velocity < 0f)
        {
            velocity = 0f;
            isFrontStop = false;
            isRearStop = false;
        }
    }

    private void Update()
    {
        if (isLineMovement)
        {
            if (velocity == 0f)
            {
                isLineMovement = false;
                BallUIOn();
            }
        }
        else if (isRotationalMovement)
        {
            if (ballManager.velocity == 0f)
            {
                isRotationalMovement = false;
                BallUIOn();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!TestLake.isOpen)
            {
                TestLake.isOpen = true;
                temp.SetActive(true);
            }
            else
            {
                TestLake.isOpen = false;
                temp.SetActive(false);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (ballManager.isRight)
            {
                ballManager.isRight = false;
                ballManager.isRightStop = true;
                isRotationalMovement = true;
            }
            else if (ballManager.isLeft)
            {
                ballManager.isLeft = false;
                ballManager.isLeftStop = true;
                isRotationalMovement = true;
            }
            else if (isFront)
            {
                isFront = false;
                isFrontStop = true;
                isLineMovement = true;
            }
            else if (isRear)
            {
                isRear = false;
                isRearStop = true;
                isLineMovement = true;
            }
        }
        else if (collision.gameObject.CompareTag("Finish"))
        {
            StartCoroutine(FinishCoroutine());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
        {
            OnClickRestart();
        }
    }

    IEnumerator FinishCoroutine()
    {
        ButtonCanvas.SetActive(false);
        MemoCanvas.SetActive(false);
        Camera mainCamera = Camera.main;
        while (Vector3.Distance(transform.position, Vector3.zero) > 0.2f)
        {
            mainCamera.orthographicSize -= 0.05f;
            transform.localScale *= 0.95f;
            velocity *= 0.95f;
            ballManager.velocity *= 0.95f;
            yield return delay;
        }
        ballManager.isRight = false;
        ballManager.isLeft = false;
        isFront = false;
        isRear = false;
        ++LakeManager.currentPhase;
        if (LakeManager.currentPhase == 5)
        {
            TestLake.isOpen = true;
        }

        Dialogue_Proceeder.instance.UpdateCurrentDiaIDPlus1(); //대사 인덱스+1 슬쩍 넣어놓고 갑니다 ㅎ
        SceneManager.LoadScene("Mental_World_Map");
    }

    public void StopBall()
    {
        ballManager.isLeft = false;
        ballManager.isRight = false;
        isFront = false;
        isRear = false;
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BallUIOn()
    {
        ballButtonPos.position = transform.position;
        leftButton.SetActive(true);
        rightButton.SetActive(true);
        FrontButton.SetActive(true);
        RearButton.SetActive(true);
    }

    private void BallUIOff()
    {
        leftButton.SetActive(false);
        rightButton.SetActive(false);
        FrontButton.SetActive(false);
        RearButton.SetActive(false);
    }

    public void OnClickStart()
    {
        isFront = true;
    }

    public void OnClickRightButton()
    {
        BallUIOff();
        ballManager.isRight = true;
    }
    public void OnClickLeftButton()
    {
        BallUIOff();
        ballManager.isLeft = true;
    }
    public void OnClickFrontButton()
    {
        BallUIOff();
        isFront = true;
    }
    public void OnClickRearButton()
    {
        BallUIOff();
        isRear = true;
    }




    public void OnClickLevel(int level)
    {
        LakeManager.currentPhase = level;
        TestLake.isOpen = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnClickExit()
    {
        Application.Quit();
    }
}