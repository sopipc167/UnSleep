using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class DiaEvent : MonoBehaviour
{
    public GameObject Dialogue_system_manager;

    public TextManager TM;
    public GameObject[] ob;
    public GameObject[] Dia;
    public GameObject FirstDia;
    public GameObject SecondDia;
    public int diaIndex;
    public int diaGroupIndex;
    public int EventNum;

    public bool isFirst = true;

    public AudioClip[] audioClip;
    public AudioSource audioSource;

    public FadeInOut fadeinout;
    public Image Fade;
    public BlinkAnimation BA;

    public int next_flase;
    public int next_true;

    bool isBearAppear;
    public Animator anim_b;
    public Player player;

    private Dialogue_Proceeder dp;

    public bool isOver;
    public Text gameOver;
    public bool isFollow;
    public bool isEnd;

    public struct ob_move
    {
        public GameObject ob;
        public bool isMove;
        public float speed;
        public int direction;
        public Vector3 endPoint;
        public int image_dir;

        public ob_move(GameObject _ob, bool _m, float _s, int _d, Vector3 _e, int _i)
        {
            ob = _ob;
            isMove = _m;
            speed = _s;
            direction = _d;
            endPoint = _e;
            image_dir = _i;
        }
    }
    List<ob_move> ob_lst = new List<ob_move>();

    public bool isMade_b;
    public bool isMade_p;

    public Transform[] tPos;
    public bool isChair;
    public DiaPlayer dia_p;

    public Gome gome;
    public bool isMini;
    public Image suprise;
    public Image block;

    public MovieEffect movieFrame;

    public CameraManager cm;
    public BoxCollider2D chair;

    public Transform[] playerPos;
    public Transform[] gomePos;

    void Start()
    {
        dp = Dialogue_Proceeder.instance;
        EventNum = 100;
        //next_flase = 700;
        //next_true = 699;
        diaIndex = dp.CurrentDiaIndex;
        diaGroupIndex = TM.Dia_Id;
    }


    void Update()
    {
        diaGroupIndex = TM.Dia_Id;

        if ((diaGroupIndex != dp.CurrentDiaID || diaIndex != dp.CurrentDiaIndex || TM.isEnd) && !isFirst)
        {
            Debug.Log("diaNext: " + EventNum);
            if (EventNum == 0)
                nextLevel();
            else if (EventNum == 1)
            {
                Shadow(false);
            }
            else if (EventNum == 2)
                Sound(100);
            else if (EventNum == 3)
            {
                Color tmp = Fade.color;
                tmp.a = 255;
                Fade.color = tmp;
            }
            else if (EventNum == 4)
                Move(2, new Vector3(7.54f, -0.95f, 0), new Vector3(0, 0, 0));
            else if (EventNum == 5)
            {
                ob[1].SetActive(false);
                if (diaGroupIndex == 728)
                {
                    Dia[35].SetActive(false);
                    Dia[34].SetActive(true);
                    Dia[36].SetActive(true);
                }
            }
            else if (EventNum == 6)
            {
                ob[3].SetActive(false);
                ob[4].SetActive(true);
                ob[5].SetActive(true);
            }
            else if (EventNum == 7)
            {
                suprise.enabled = false;
            }
            else if (EventNum == 8)
            {
                gome.targetPos = player.transform.position;
                gome.isFollow = true;
                gome.isStart = true;
                player.isStop = false;
                isFollow = true;
            }

            TM.isEnd = false;
            isFirst = true;
            EventNum = 100;
        }

        if (isFirst)
        {
            if (TM.con != "Sound0" || TM.con != "Sound1" || TM.con == "Sound2")
                Setting();

            if (TM.con == "Shadow")
            {
                Shadow(true);
                TM.con = null;
                EventNum = 1;
            }
            else if (TM.con == "LightOff")
            {
                ob[7].SetActive(false);
            }
            else if (TM.con == "BearUp")
            {
                EventNum = 100;
                Move(1, new Vector3(10.15f, 0.58f, 0), new Vector3(0, 0, 0));
            }
            else if (TM.con == "Sound0" || TM.con == "Sound1" || TM.con == "Sound2")
            {
                EventNum = 2;
                if (TM.con == "Sound0")
                    Sound(0);
                else if (TM.con == "Sound1")
                    Sound(1);
                else if (TM.con == "Sound2")
                    Sound(2);

                Setting();
            }
            else if (TM.con == "Next")
            {
                EventNum = 0;
            }
            else if (TM.con == "BlinkOpen")
            {
                Color tmp = Fade.color;
                tmp.a = 0;
                Fade.color = tmp;
                BA.BlinkOpen();
                TM.con = null;
            }
            else if (TM.con == "BlinkClose")
            {
                BA.isSeven_Close = true;
                BA.BlinkClose();
                TM.con = null;
                EventNum = 3;
            }
            else if (TM.con == "ChairMove")
            {
                EventNum = 4;
            }
            else if (TM.con == "BearDis")
            {
                if (diaGroupIndex == 738)
                {
                    ob[5].SetActive(false);
                }
                else
                    EventNum = 5;
            }
            else if (TM.con == "WindowOpen")
            {
                EventNum = 6;
            }
            else if (TM.con == "BearBig")
            {
                if (!isBearAppear)
                {
                    isBearAppear = true;
                    Move(5, new Vector3(-7.18f, -1.32f, 0), new Vector3(0, 0, 0));
                    ob[5].SetActive(true);
                    Dialogue_system_manager.GetComponent<TextManager>().Increasediaindex = true;
                }
                else
                {
                    Move(5, new Vector3(-7.18f, -0.29f, 0), new Vector3(0, 0, 0));
                    ob[5].transform.DOScaleX(-2.3f, 0.5f);
                    ob[5].transform.DOScaleY(2.3f, 0.5f);
                    Dialogue_system_manager.GetComponent<TextManager>().Increasediaindex = true;
                }
            }
            else if (TM.con == "BearMove")
            {
                if (!isMade_b)
                {
                    Vector3 targetPos = player.transform.position;
                    targetPos.x -= 3;
                    ob_move bear = new ob_move(ob[5], true, 3.5f, 1, targetPos, 1);
                    gome.ChangeTarget(targetPos);
                    ob_lst.Add(bear);
                    Dia[37].SetActive(true);
                    isMade_b = true;
                    gome.isStart = true;
                    anim_b.SetBool("isMove", true);
                    block.enabled = true;
                }
                else if (isMade_b && !ob_lst[0].isMove)
                {
                    Vector3 targetPos = player.transform.position;
                    targetPos.x -= 9;
                    Move(5, targetPos, new Vector3(0, 0, 0));
                    gome.targetPos = targetPos;
                }

                TM.con = null;
            }
            else if (TM.con == "PlayerMove")
            {
                if (!isMade_p)
                {
                    //Debug.Log("플레이어 리스트 추가");
                    ob_move domoon = new ob_move(ob[6], true, 5.0f, 1, tPos[0].position, 1);
                    ob_lst.Add(domoon);
                    isMade_p = true;
                }
                else if (isMade_p && !ob_lst[1].isMove)
                {
                    //Debug.Log("player Move");
                    chair.enabled = true;
                    changeEndPoint(ob_lst, 1, tPos[2].position);
                    changeDirection(ob_lst, 1, -1);
                    changeIsMove(ob_lst, 1, true);
                    cm.isChoose = false;
                    EventNum = 8;
                }

                TM.con = null;
                block.enabled = true;
                player.animator.SetBool("isMove", true);
                player.isSeven = true;
            }
            else if (TM.con == "SceneOver")
            {
                next_flase = 711;
                next_true = 710;
                dia_p.diaScene3.SetActive(false);
                dia_p.diaScene4.SetActive(true);
                Vector3 targetPos = player.transform.position;
                targetPos.x -= 2.5f;
                gome.ChangeTarget(targetPos);
                block.enabled = true;
                gome.isFollow = false;
                gome.isStart = true;
            }
            else if (TM.con == "MiniGame")
            {
                if (!isMini)
                {
                    gome.isMinigame = true;
                    Vector3 targetPos = player.transform.localPosition;
                    targetPos.x -= 4;
                    changeEndPoint(ob_lst, 1, targetPos);
                    changeDirection(ob_lst, 1, 1);
                    changeIsMove(ob_lst, 1, true);
                    block.enabled = true;
                    player.animator.SetBool("isMove", true);
                    player.isSeven = true;
                    Debug.Log(player.isSeven);
                    isMini = true;
                    gome.speed = 2.5f;
                }
                //EventNum = 8;
            }
            else if (TM.con == "Eight")
            {
                EventNum = 8;
            }
            else if (TM.con == "LightOn")
            {
                ob[7].SetActive(true);
                gome.isStart = false;
            }
            else if (TM.con == "Suprise")
            {
                suprise.enabled = true;
                EventNum = 7;
            }
            else if (TM.con == "FrameOut")
            {
                movieFrame.MovieFrameout();
            }
            else if(TM.con == "Choose")
            {
                fadeinout.Blackout_Func(0.7f);
                player.isSeven = true;
                player.targetPos = tPos[1].position;
                Move(6, tPos[1].position, new Vector3(0, 180, 0));
                cm.isChoose = true;
                player.isSeven = false;
                TM.con = null;
                PlayerPrefs.SetInt("savePoint", 1);
                Dia[37].SetActive(true);
                Dia[38].SetActive(true);
            }
            else if(TM.con == "Gome")
            {
                Vector3 target = player.transform.position;
                target.x -= 3;
                gome.ChangeTarget(target);
                gome.isFollow = false;
                gome.isStart = true;
                //EventNum = 8;
            }
            else if(TM.con == "GameOver")
            {
                StartCoroutine(GameOver());
            }
        }

        if (ob_lst.Count != 0)
        {
            for (int i = 0; i < ob_lst.Count; i++)
            {
                ob_move tmp = ob_lst[i];
                if (tmp.isMove)
                {
                    if ((tmp.ob.transform.position.x >= tmp.endPoint.x && tmp.direction > 0)
                        || (tmp.ob.transform.position.x <= tmp.endPoint.x && tmp.direction < 0))
                    {
                        if (i == 0)
                        {
                            anim_b.SetBool("isMove", false);
                        }
                        else if (i == 1)
                        {
                            player.targetPos = tmp.endPoint;
                            player.isSeven = false;
                            player.animator.SetBool("isMove", false);
                        }

                        block.enabled = false;
                        changeIsMove(ob_lst, i, false);
                        moveEnd();
                    }
                    else
                    {
                        //tmp.ob.transform.position += new Vector3(tmp.speed * Time.deltaTime * tmp.direction, 0, 0);
                        tmp.ob.transform.position = Vector3.MoveTowards(tmp.ob.transform.position, tmp.endPoint, tmp.speed * Time.deltaTime);
                        if (tmp.image_dir == 1)
                            tmp.ob.transform.eulerAngles = new Vector3(0, 0, 0);
                        else
                            tmp.ob.transform.eulerAngles = new Vector3(0, 180, 0);
                    }
                }
            }
        }
    }

    public void moveEnd()
    {
        Dialogue_system_manager.GetComponent<TextManager>().Increasediaindex = true;
    }

    public void changeIsMove(List<ob_move> lst, int index, bool value)
    {
        ob_move temp = lst[index];
        temp.isMove = value;
        lst[index] = temp;
        Debug.Log("changeIsMove");
    }

    public void changeDirection(List<ob_move> lst, int index, int dir)
    {
        ob_move temp = lst[index];
        temp.image_dir = dir;
        lst[index] = temp;
    }

    public void changeEndPoint(List<ob_move> lst, int index, Vector3 pos)
    {
        ob_move temp = lst[index];
        temp.endPoint = pos;
        lst[index] = temp;
        Debug.Log("changeEndPoint");
    }


    void Setting()
    {
        isFirst = false;
        diaIndex = dp.CurrentDiaIndex;
    }

    public void nextLevel()
    {
        Dia[diaGroupIndex - next_flase].SetActive(false);
        Dia[diaGroupIndex - next_true].SetActive(true);
    }

    public void Shadow(bool isOn)
    {
        if (isOn)
        {
            Debug.Log("shadow_true");
            ob[0].SetActive(isOn);
        }
        else if(!isOn)
        {
            Debug.Log("shadow_false");
            ob[0].SetActive(isOn);
            FirstDia.SetActive(false);
            SecondDia.SetActive(true);
        }
    }

    public void Move(int index, Vector3 pos, Vector3 angle)
    {
        ob[index].transform.localPosition = pos;
        ob[index].transform.eulerAngles = angle;
    }

    public void Sound(int soundNum)
    {
        if(soundNum < 2)
            audioSource.clip = audioClip[soundNum];

        switch (soundNum)
        {
            case 0:
                //Debug.Log("Sound0");
                audioSource.panStereo = 1;
                audioSource.volume = 0.3f;
                audioSource.Play();
                return;
            case 1:
                //Debug.Log("Sound1");
                audioSource.panStereo = 1;
                audioSource.volume = 0.7f;
                audioSource.Play();
                return;
            case 2:
                //Debug.Log("Sound2");
                audioSource.panStereo = 0.7f;
                audioSource.volume = 0.7f;
                audioSource.Play();
                return;
            default:
                //Debug.Log("Stop");
                audioSource.Stop();
                return;
        }
    }

    public void GameOver_s()
    {
        if (!isOver && isFollow)
        {
            StartCoroutine(GameOver());
        }
    }

    IEnumerator GameOver()
    {
        isOver = true;
        player.targetPos = player.transform.position;
        player.isStop = true;
        gome.isStart = false;
        fadeinout.Fade_In();
        yield return new WaitForSeconds(2.5f);
        Color tmp = gameOver.color;
        tmp.a = 1;
        gameOver.color = tmp;
        gameOver.DOText(gameOver.text, 0.5f);
        yield return new WaitForSeconds(0.5f);

        PlayerPrefs.SetInt("isGameOver", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


        //프로시더에서 완료된 대화묶음 지우기
        if (dp.Complete_Condition.Contains(744))
        {
            int i = 0;

            if (dp.Complete_Condition.Contains(749))
                i = 749;
            else
                i = 742;

            while (dp.Complete_Condition.Contains(i))
            {
                dp.RemoveCompleteCondition(i);
                i++;
            }
        }
        isOver = false;
    }
}
