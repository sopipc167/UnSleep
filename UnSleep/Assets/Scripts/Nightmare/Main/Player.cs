using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player instance;

    public SpriteRenderer sprite;
    public float speed;
    public float Speed;
    public Vector3 mousePos, transPos, targetPos;
    public bool isStop;

    public bool isEvent;

    public bool isRoadEnd;
    public CameraManager C;

    public bool isMiniGame;
    public float speed_M;
    public bool isFall;
    public bool isMonster;

    public ObManager Ob_M;
    public Obstruction Ob;

    public GroundScroller[] GS;

    public Animator animator;
    public Collider2D col;
    public bool isSeven;

    public DiaEvent dia;

    public BoxCollider2D Seven;
    public BoxCollider2D Noise;

    public bool isAuto;

    public GameObject surprise;

    public Image sadFace;

    void Start()
    {
        instance = this;
        Speed = speed;
        
        if (isMiniGame)
        {
            transform.position = new Vector3(-1.6f, 0.05f, 0);
            animator.SetBool("isRun", true);
        }
        else
        {
            if (PlayerPrefs.GetInt("isGameOver") == 0)
            {
                targetPos = new Vector3(-5.5f, -1.5f, 0);
            }
            else if (PlayerPrefs.GetInt("isGameOver") == 1)
            {
                targetPos = dia.playerPos[PlayerPrefs.GetInt("savePoint")].position;
                transform.position = targetPos;
                isSeven = false;
            }

            animator.SetBool("isSeven", true);
        }

    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !Ob_M.isFull && !isStop)
        {
            TransTargetPos();
        }

        Vector3 tmp = transform.position;
        if (isMiniGame)
        {
            if (targetPos.y > -1.1f)
                tmp.y = 1.0f;
            else if (targetPos.y > -3.6f)
                tmp.y = -0.7f;
            else
                tmp.y = -2.45f;

            transform.position = Vector3.MoveTowards(transform.position, tmp, Time.deltaTime * speed_M);

        }
        else if (!isSeven)
        {
            if (targetPos.y > -0.92f)
            {
                targetPos.y = -0.92f;
            }
            if (targetPos.y < -2.85f)
            {
                targetPos.y = -2.85f;
            }


            if (transform.position == targetPos)
            {
                animator.SetBool("isMove", false);
                if (isAuto)
                {
                    dia.moveEnd();
                    isAuto = false;
                }
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
        }


        //Scene2 엔딩 도문이 뛰는거
        if (Ob_M.isStopM)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(5.98f, 0.07f, 0), Time.deltaTime * speed);
        }
    }


    void TransTargetPos()
    {
        mousePos = Input.mousePosition;
        transPos = Camera.main.ScreenToWorldPoint(mousePos);
        if (!isMiniGame)
        {
            if (transPos.x > transform.position.x)
            {
                Vector3 tmp = new Vector3(0, 180, 0);
                transform.eulerAngles = tmp;
            }
            else
            {
                Vector3 tmp = new Vector3(0, 0, 0);
                transform.eulerAngles = tmp;
            }
        }

        targetPos = new Vector3(transPos.x, transPos.y, 0);
        animator.SetBool("isMove", true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ob_N")
        {

            if(!isFall)
                StartCoroutine(Stop());
        }

        if(collision.gameObject.tag == "Ob_S")
        {
            if (Ob_M.Gauge.value >= 0.1f)
                Ob_M.Gauge.value -= 0.1f;
            else
                Ob_M.Gauge.value = 0;
        }

        if(collision.gameObject.tag == "Monster")
        {
            //sprite.enabled = false;
            if(!isMonster)
                StartCoroutine(MonsterEat());
        }

        if(collision.gameObject.tag == "Gome")
        {
            dia.GameOver_s();
        }

        if(collision.gameObject.tag == "Unperformed")
        {
            dia.unperform();
        }
    }

    public void BackGroundStop()
    {
        for (int i = 0; i < 4; i++)
        {
            GS[i].speed = 0;
        }
    }

    IEnumerator Stop()
    {
        isFall = true;
        //Ob.enabled = false;
        isStop = true;

        sadFace.enabled = true;

        if (Ob_M.Gauge.value >= 0.1f)
            Ob_M.Gauge.value -= 0.1f;
        else
            Ob_M.Gauge.value = 0;
        
        animator.SetBool("isStop", true);
        if (!Ob_M.isOne)
        {
            Ob_M.ObList[1].GetComponent<Obstruction>().enabled = false;
            Ob_M.ObList[2].GetComponent<Obstruction>().enabled = false;
        }
        else
            Ob_M.ObList[0].GetComponent<Obstruction>().enabled = false;

        BackGroundStop();

        yield return new WaitForSeconds(0.5f);
        sadFace.enabled = false;
        isStop = false;
        animator.SetBool("isStop", false);
        if (!Ob_M.isOne)
        {
            Ob_M.ObList[1].GetComponent<Obstruction>().enabled = true;
            Ob_M.ObList[2].GetComponent<Obstruction>().enabled = true;
        }
        else
            Ob_M.ObList[0].GetComponent<Obstruction>().enabled = true;
        //Ob.enabled = true;

        for (int i = 0; i < 4; i++)
        {
            GS[i].SpeedBack();
        }


        yield return new WaitForSeconds(0.5f);
        isFall = false;
    }

    IEnumerator MonsterEat()
    {
        isMonster = true;
        Ob_M.Eat();
        yield return new WaitForSeconds(0.55f);
        Ob_M.animator.SetTrigger("isStop");
        sprite.enabled = false;
    }

}
