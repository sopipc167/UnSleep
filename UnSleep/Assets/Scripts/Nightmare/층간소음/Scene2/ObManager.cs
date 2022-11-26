using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ObManager : MonoBehaviour
{
    public Transform[] CPos;
    public Transform[] CPos_S;
    public Slider Gauge;
    public Player player;
    public float speed;

    public GameObject[] ob_N;
    int ob1_pos;
    public GameObject ob_S;

    public bool isCreate;
    public bool isCreateS;
    public bool isOne;

    public Obstruction[] ob;
    public CameraManager cm;

    public GroundScroller[] gs;
    public  List<GameObject> ObList = new List<GameObject>();
    public GameObject Obstruction;
    public int i;

    public bool isFull;
    public GameObject Monster;
    public bool isStopM;
    public GameObject Native;
    public Transform target;
    public NativeMove NM;
    public Animator animator;

    public NoiseManager Noise_M;
    public Image BG;

    public bool isEnding;

    public SpriteRenderer[] warning;
    public string[] road;

    public Ease Movement;
    public MovieEffect frame;
    public bool isClear;

    void Start()
    {
        Gauge.value = 0;
        SoundManager.Instance.PlayBGM("Dream Of You");
    }

    void Update()
    {
        if(Gauge.value < 0.95f)
        {
            Gauge.value += Time.deltaTime / 60;
            speed = 10 + (Gauge.value * 5);
        }
        else if(Gauge.value == 1.0f && !isFull)
        {
            player.targetPos.y = -1.4f;
            Native.transform.position = Vector3.MoveTowards(Native.transform.position, target.position, 10 * Time.deltaTime);
            
            if(!isEnding)
                StartCoroutine(GameEnding());
        }
        else
        {
            Gauge.value += Time.deltaTime / 60;
            speed = 10;
        }

        //Ending
        if (isFull && !isStopM && !isClear)
        {
            isClear = true;
            StartCoroutine(GameClear());
            NM.isStop = true;            
        }


        ob[0].speed_ori = speed;
        ob[1].speed_ori = speed;
        if (!player.isStop && !isFull)
        {
            for(int i = 0; i< 4; i++)
                gs[i].speed = speed;
        }

        if (Gauge.value > 0.1f)
        {
            if (isOne && !isCreateS && Gauge.value < 0.95f)
            {
                StartCoroutine(CreateOB_S1());
            }
        }

        if (!isCreate && Gauge.value < 0.95f)
        {
            int obNum = Random.Range(0, 100);
            if (Gauge.value >= 2 / 3)
            {
                if (obNum <= 90 && obNum > 30)
                    StartCoroutine(CreateOB2());
                else
                    StartCoroutine(CreateOB1());
            }
            else if (Gauge.value >= 1 / 3)
            {
                if (obNum <= 30)
                    StartCoroutine(CreateOB2());
                else
                    StartCoroutine(CreateOB1());
            }
            else
            {
                StartCoroutine(CreateOB1());
            }
        }

        if (cm.isThree)
            Gauge.gameObject.SetActive(false);
        else
            Gauge.gameObject.SetActive(true);
    }

    IEnumerator GameClear()
    {
        yield return new WaitForSeconds(1.5f); //도문이가 도망치는 시간
        player.surprise.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        player.surprise.SetActive(false);
        isStopM = true; //도문이 이동
        player.BackGroundStop(); //배경 및 바닥 멈춤
        yield return new WaitForSeconds(0.5f);
        Monster.transform.DOLocalMoveX(-58.05f, 0.2f).SetEase(Movement); //북귀신 이동
        yield return new WaitForSeconds(1.0f);
        SoundManager.Instance.FadeOutBGM(delay:1.0f);
        Noise_M.coStart(4, 5);
        frame.MovieFrameout();
        Color tmp = BG.color;
        tmp.a = 255;
        BG.color = tmp;
        Noise_M.TM.DiaUI.SetActive(true);
        Noise_M.TM.Increasediaindex = true;
    }

    IEnumerator GameEnding()
    {
        frame.MovieFrameIn();
        isEnding = true;
        player.isStop = true;
        yield return new WaitForSeconds(1.5f);
        Noise_M.TM.DiaUI.SetActive(true);
        Noise_M.TM.Increasediaindex = true;
    }

    public void Eat()
    {
        animator.SetTrigger("isEat");
    }

    IEnumerator CreateOB_S1()
    {
        isCreateS = true;
        int posNum;

        do
        {
            posNum = Random.Range(0, 3);
        } while (posNum == ob1_pos);


        for(int i = 0; i < 3; i++)
        {
            warning[posNum].enabled = true;
            yield return new WaitForSeconds(0.1f);
            warning[posNum].enabled = false;
            yield return new WaitForSeconds(0.1f);
        }

        Instantiate(ob_S, CPos_S[posNum].position, Quaternion.Euler(0, 0, 0));
        yield return new WaitForSeconds(2.5f);
        isCreateS = false;
    }

    IEnumerator CreateOB1() //장애물 한개 등장
    {
        isCreate = true;
        isOne = true;
        ob1_pos = Random.Range(0, 3); //장애물 위치 랜덤
        int ran = Random.Range(0, 2); //장애물 종류 랜덤
        Obstruction = Instantiate(ob_N[ran], CPos[ob1_pos].position, Quaternion.Euler(0, 0, 0));
        ObList[0] = Obstruction;

        yield return new WaitForSeconds(3);
        isCreate = false;
    }

    IEnumerator CreateOB2() //장애물 두개 등장
    {
        isCreate = true;
        isOne = false;

        int posNum1 = Random.Range(0, 3);
        int posNum2;

        do  //장애물 두개의 위치가 같을 경우 다른 위치가 나올때까지 다시 랜덤
        {
            posNum2 = Random.Range(0, 3);
        } while (posNum2 == posNum1);


        //장애물 종류 랜덤
        int ran1 = Random.Range(0, 2);
        int ran2 = Random.Range(0, 2);
        
        Obstruction = Instantiate(ob_N[ran1], CPos[posNum1].position, Quaternion.Euler(0, 0, 0));
        ObList[1] = Obstruction;
        Obstruction = Instantiate(ob_N[ran2], CPos[posNum2].position, Quaternion.Euler(0, 0, 0));
        ObList[2] = Obstruction;

        yield return new WaitForSeconds(3);
        isCreate = false;
    }


    public void Skip()
    {
        Gauge.value = 0.95f;
    }
}
