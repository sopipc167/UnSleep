using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    public SpriteRenderer sprite;
    public float speed;
    public float Speed;
    public Vector3 mousePos, transPos, targetPos;
    public bool isStop;

    public GameObject dropObject;
    public Transform dropStart;
    public GameObject[] dropPos;
    int i;

    public bool isEvent;

    public bool isRoadEnd;
    public CameraManager C;

    public GameObject appearObject;
    public Transform appearStart;

    public bool isMiniGame;
    public float speed_M;

    public ObManager Ob_M;
    public Obstruction Ob;

    public GroundScroller[] GS;

    public Animator animator;
    public Collider2D col;
    public bool isSeven;

    public DiaEvent dia;

    public BoxCollider2D Seven;
    public BoxCollider2D Noise;

    void Start()
    {
        instance = this;
        Speed = speed;
        if (PlayerPrefs.GetInt("isGameOver") == 0)
        {
            targetPos = new Vector3(0.26f, 0.07f, 0);
        }
        else if(PlayerPrefs.GetInt("isGameOver") == 1)
        {
            targetPos = dia.playerPos[PlayerPrefs.GetInt("savePoint")].position;
            transform.position = targetPos;
        }

        if (isMiniGame)
        {
            transform.position = new Vector3(0.26f, 0.05f, 0);
            animator.SetBool("isRun", true);
        }
        else
        {
            animator.SetBool("isSeven", true);
        }
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !Ob_M.isFull  && !isStop)
        {
            TransTargetPos();
        }

        Vector3 tmp = transform.position;
        if (isMiniGame)
        {
            if (targetPos.y > 0.18f)
                tmp.y = 2.94f;
            else if (targetPos.y > -2.8f)
                tmp.y = 0.05f;
            else
                tmp.y = -2.42f;

            transform.position = Vector3.MoveTowards(transform.position, tmp, Time.deltaTime * speed_M);

        }
        else if(!isSeven)
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
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
        }

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
        if (collision.gameObject.tag == "Drop" && !isEvent)
        {
            StartCoroutine(Event(true, 0.8f));
            Instantiate(dropObject, dropStart.position, Quaternion.Euler(0, 0, 0));
        }

        if (collision.gameObject.tag == "RoadEnd")
        {
            StartCoroutine(RoadEnd());
        }

        if(collision.gameObject.tag == "Appear")
        {
            StartCoroutine(Event(false, 0.8f));
            Instantiate(appearObject, appearStart.position, Quaternion.Euler(0, 0, 0));
        }

        if(collision.gameObject.tag == "Ob_N")
        {
            if(Ob_M.Gauge.value >= 0.1f)
                Ob_M.Gauge.value -= 0.1f;
            else
                Ob_M.Gauge.value = 0;
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
            sprite.enabled = false;
        }

        if(collision.gameObject.tag == "Gome")
        {
            dia.GameOver_s();
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
        //Ob.enabled = false;
        isStop = true;
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
    }

    IEnumerator Event(bool isOnce, float runningTime)
    {
        isEvent = true;
        speed = 0;
        if (isOnce)
        {
            dropPos[i].SetActive(false);
            i++;
        }
        yield return new WaitForSeconds(runningTime);
        speed = Speed;
        isEvent = false;
    }

    IEnumerator RoadEnd()
    {
        C.isStop = !C.isStop;
        yield return new WaitForSeconds(0.2f);
        isRoadEnd = !isRoadEnd;
    }    
}
