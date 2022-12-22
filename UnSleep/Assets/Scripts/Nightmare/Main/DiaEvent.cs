using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class DiaEvent : MonoBehaviour
{
    public GameObject Dialogue_system_manager;
    public string content;

    public TextManager TM;
    public GameObject[] ob;
    public GameObject[] Dia;
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
    public int MovePoint = 0;
    public Transform[] gomePos;

    public Image ending_bg;
    public Image eye;

    public bool isMovie;

    void Start()
    {
        dp = Dialogue_Proceeder.instance;
        EventNum = 100;
        diaIndex = dp.CurrentDiaIndex;
        diaGroupIndex = TM.Dia_Id;
    }


    void Update()
    {
        diaGroupIndex = TM.Dia_Id;

        if ((diaGroupIndex != TM.Dia_Id || diaIndex != dp.CurrentDiaIndex || TM.isEnd) && !isFirst)
        {
            Debug.Log("con_EventNum: " + EventNum);
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
                if (diaGroupIndex == 729)
                {
                    Dia[34].SetActive(false);
                    Dia[35].SetActive(true);
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
            else if(EventNum == 9)
            {
                moveEnd();
            }
            else if (EventNum == 10)
            {
                player.enabled = false;
                Move(9, tPos[5].localPosition, ob[9].transform.eulerAngles);
                ob[9].transform.localScale = new Vector3(1.3f, 1.3f, 1.0f);
            }
            else if(EventNum == 13)
            {
                movieFrame.MovieFrameout();
                TM.isMovieIn = false;
            }


            if (isMovie)
            {
                movieFrame.MovieFrameout();
                isMovie = false;
            }

            TM.isEnd = false;
            isFirst = true;
            Debug.Log("con_isFirst");
            EventNum = 100;
        }

        if (isFirst)
        {
            if (content != "Sound0" || content != "Sound1" || content != "Sound2")
            {
               
                Setting();
            }

            if (content == "Shadow")
            {
                Shadow(true);
                content = null;
                EventNum = 1;
            }
            else if (content == "LightOff")
            {
                ob[8].SetActive(true);
                ob[7].SetActive(false);
            }
            else if (content == "BearUp")
            {
                Move(1, new Vector3(10.15f, 0.58f, 0), new Vector3(0, 0, 0));
            }
            else if (content == "Sound0" || content == "Sound1" || content == "Sound2")
            {
                EventNum = 2;
                if (content == "Sound0")
                    Sound(0);
                else if (content == "Sound1")
                    Sound(1);
                else if (content == "Sound2")
                    Sound(2);

                Setting();
            }
            else if (content == "Next")
            {
                content = null;
                EventNum = 0;
            }
            else if (content == "BlinkOpen")
            {
                Color tmp = Fade.color;
                tmp.a = 0;
                Fade.color = tmp;
                BA.BlinkOpen();
                content = null;
            }
            else if (content == "BlinkClose")
            {
                BA.isSeven_Close = true;
                BA.BlinkClose();
                content = null;
                EventNum = 3;
            }
            else if (content == "ChairMove")
            {
                EventNum = 4;
            }
            else if (content == "BearDis")
            {
                if (diaGroupIndex == 738)
                {
                    ob[5].SetActive(false);
                }
                else
                    EventNum = 5;
            }
            else if (content == "WindowOpen")
            {
                EventNum = 6;
            }
            else if (content == "BearBig")
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
                    Dialogue_system_manager.GetComponent<TextManager>().Increasediaindex = false;
                    StartCoroutine(Bigger());
                }
            }
            else if(content == "BearMove")
            {
                if(diaGroupIndex == 743)
                {
                    gome.isStart = false;
                    gome.transform.position = tPos[3].transform.position;
                    gome.ChangeTarget(tPos[3].transform.position);
                    gome.isStart = true;
                }
                else
                {
                    Vector3 targetPos = player.transform.position;
                    targetPos -= new Vector3(1.5f, 0, 0);
                    gome.ChangeTarget(targetPos);
                    gome.isStart = true;
                }
            }
            else if(content == "PlayerMove")
            {
                if (diaGroupIndex == 743)
                {
                    PlayerMove(2);
                    EventNum = 8;
                }
                else
                {
                    PlayerMove(0);
                }

                content = null;
            }
            else if (content == "SceneOver")
            {
                dia_p.diaScene3.SetActive(false);
                dia_p.diaScene4.SetActive(true);
                Vector3 targetPos = player.transform.position;
                targetPos.x -= 2.5f;
                gome.ChangeTarget(targetPos);
                block.enabled = true;
                gome.isFollow = false;
                gome.isStart = true;
            }
            else if (content == "MiniGame")
            {
                gome.isMinigame = true;
                PlayerMove(4);
                isMini = true;
                gome.speed = 2.5f;
            }
            else if (content == "Eight")
            {
                PlayerPrefs.SetInt("savePoint", 2);
                EventNum = 8;
                content = null;
            }
            else if (content == "LightOn")
            {
                ob[7].SetActive(true);
                gome.isStart = false;
            }
            else if(content == "Hide")
            {
                Debug.Log("Hide");
                EventNum = 10;
                content = null;
            }
            else if (content == "Suprise")
            {
                suprise.enabled = true;
                content = null;
                EventNum = 7;
            }
            else if (content == "FrameOut")
            {
                EventNum = 13;
                content = null;
            }
            else if (content == "Choose")
            {
                fadeinout.Blackout_Func(0.7f);
                player.transform.eulerAngles = new Vector3(0, 180, 0);
                PlayerMove(1);
                content = null;
                PlayerPrefs.SetInt("savePoint", 1);
                Dia[37].SetActive(true);
                Dia[38].SetActive(true);
                Dia[39].SetActive(true);
                EventNum = 13;
            }
            else if (content == "Gome")
            {
                Vector3 target = player.transform.position;
                target.x -= 3;
                gome.ChangeTarget(target);
                gome.isFollow = false;
                gome.isStart = true;
            }
            else if(content == "LightBlink1")
            {
                StartCoroutine(LBlink(1));
            }
            else if(content == "LightBlink2")
            {
                StartCoroutine(LBlink(2));
            }
            else if (content == "GameOver")
            {
                StartCoroutine(GameOver());
            }
            else if(content == "Ending")
            {
                fadeinout.Blackout_Func(0.3f);
                StartCoroutine(Ending());
            }
        }
    }

    IEnumerator LBlink(int times)
    {
        Dialogue_system_manager.GetComponent<TextManager>().Increasediaindex = false;
        for (int i = 0; i < times; i++)
        {
            Color tmp = Fade.color;
            tmp.a = 0.7f;
            Fade.color = tmp;
            yield return new WaitForSeconds(0.3f);
            tmp.a = 0;
            Fade.color = tmp;
            yield return new WaitForSeconds(0.3f);
        }
        Dialogue_system_manager.GetComponent<TextManager>().Increasediaindex = true;
    }

    IEnumerator Bigger()
    {
        Debug.Log("Bigger");
        //Debug.Log("Gome_X: " + ob[5].transform.localScale.x);
        ob[5].transform.DOScaleX(-2.3f, 0.5f);
        ob[5].transform.DOScaleY(2.3f, 0.5f);
        yield return new WaitForSeconds(0.2f);
        Dialogue_system_manager.GetComponent<TextManager>().Increasediaindex = true;
    }

    IEnumerator Ending()
    {
        ending_bg.enabled = true;
        eye.enabled = true;
        yield return new WaitForSeconds(2.5f);
        fadeinout.Fade_In();
    }


    public void PlayerMove(int PM)
    {
        player.isAuto = true;
        player.targetPos = tPos[PM].transform.position;
        player.animator.SetBool("isMove", true);
        if(PM == 2)
            chair.enabled = true;
    }


    public void moveEnd()
    {
        Dialogue_system_manager.GetComponent<TextManager>().Increasediaindex = true;
    }

   

    void Setting()
    {
        Debug.Log("setting");
        isFirst = false;
        diaIndex = dp.CurrentDiaIndex;
    }

    public void nextLevel()
    {
        Debug.Log("i'm on the nextLevel: " + diaGroupIndex);
        Dia[diaGroupIndex - next_flase].SetActive(false);
        Dia[diaGroupIndex - next_true].SetActive(true);
        Debug.Log("false: " + (diaGroupIndex - next_flase));
        Debug.Log("true: " + (diaGroupIndex - next_true));
    }

    public void Shadow(bool isOn)
    {
        if (isOn)
        {
            ob[0].SetActive(isOn);
            Dialogue_system_manager.GetComponent<TextManager>().Increasediaindex = true;
        }
        else if(!isOn)
        {
            ob[0].SetActive(isOn);
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
        if (dp.Complete_Condition.Contains(743))
        {
            int i = 0;
            int max = 0;

            if (dp.Complete_Condition.Contains(749))
            {
                i = 749;
                max = 755;
            }
            else
            {
                i = 743;
                max = 744;
            }

            while (i <= max)
            {
                if(dp.Complete_Condition.Contains(i))
                    dp.RemoveCompleteCondition(i);
                i++;
            }
        }
        isOver = false;
    }
}
