using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class DiaEvent : MonoBehaviour
{
    public GameObject Dialogue_system_manager;
    public GameObject Scene1;
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
    public Image surprise;
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
    public bool isUnperform;

    public int effectIndex;

    public int outline = 0;
    public SpriteRenderer[] choose;
    public bool isChoose;
    public bool isBlink;

    private void Awake()
    {
        player.isStop = true;
        if(PlayerPrefs.GetInt("isGameOver") == 0)
            StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1.0f);
        Scene1.SetActive(true);
    }

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

            if (isChoose)
            {
                isChoose = false;
                Color full = new Vector4(255, 255, 255, 255);
                for(int i = 0; i < 2; i++)
                {
                    choose[i].enabled = false;
                    choose[i].color = full;
                }
            }

            if (EventNum == 0)
                nextLevel();
            else if (EventNum == 1)
            {
                Shadow(false);
            }
            else if (EventNum == 2)
                SoundManager.Instance.StopSE();
            else if (EventNum == 3)
            {
                Color tmp = Fade.color;
                tmp.a = 255;
                Fade.color = tmp;
                TM.EffectEnd = true;
            }
            else if (EventNum == 4)
            {
                SoundManager.Instance.PlaySE("chair");
                Move(2, new Vector3(7.54f, -0.95f, 0), new Vector3(0, 0, 0));
            }
            else if (EventNum == 5)
            {
                ob[1].SetActive(false);
                if (diaGroupIndex == 729)
                {
                    Dia[22].SetActive(false);
                    Dia[23].SetActive(false);
                    Dia[34].SetActive(false);
                    Dia[35].SetActive(true);
                    Dia[36].SetActive(true);
                }
                SoundManager.Instance.PlaySE("fuss");
            }
            else if (EventNum == 6)
            {
                SoundManager.Instance.PlaySE("WindowOpen");
                ob[3].SetActive(false);
                ob[4].SetActive(true);
                ob[5].SetActive(true);
            }
            else if (EventNum == 7)
            {
                surprise.enabled = false;
            }
            else if (EventNum == 8)
            {
                SoundManager.Instance.PlayBGM("gomeFollow");
                if (diaGroupIndex == 744)
                {
                    choose[0].enabled = true;
                    choose[1].enabled = true;
                    isChoose = true;
                }

                gome.targetPos = player.transform.position;
                gome.isFollow = true;
                gome.isStart = true;
                gome.isMinigame = false;
                player.isStop = false;
                isFollow = true;
            }
            else if (EventNum == 9)
            {
                moveEnd();
            }
            else if (EventNum == 10)
            {
                player.sprite.enabled = false;
                Move(9, tPos[5].localPosition, ob[9].transform.eulerAngles);
                ob[9].transform.localScale = new Vector3(1.3f, 1.3f, 1.0f);
            }
            else if (EventNum == 13)
            {
                isMovie = false;
                TM.isMovieIn = true;
            }
            else if (EventNum == 14)
            {
                fadeinout.Blackout_Func(0.5f);
                PlayerMove(6);
            }
            else if (EventNum == 15)
            {
                TM.EffectEnd = true;
            }
            else if (EventNum == 16)
            {
                GameOver_s();
            }
            else if (EventNum == 17)
            {
                Color tmp = Fade.color;
                tmp.a = 255;
                Fade.color = tmp;
                ob[5].SetActive(false);
            }

            if (isMovie)
            {
                Debug.Log("MovieFrameOut");
                movieFrame.MovieFrameout();
                isMovie = false;
            }

            TM.isEnd = false;
            isFirst = true;
            EventNum = 100;
        }

        if (isChoose && !isBlink)
        {
            StartCoroutine(chooseBlink());
        }

        if (isFirst)
        {
            Setting();

            if (content == "Shadow")
            {
                Shadow(true);
                content = null;
                EventNum = 1;
            }
            else if (content == "LightOff")
            {
                SoundManager.Instance.PlaySE("LightOnOff");
                ob[8].SetActive(true);
                ob[7].SetActive(false);
            }
            else if (content == "BearUp")
            {
                Move(1, new Vector3(10.15f, 0.58f, 0), new Vector3(0, 0, 0));
                Dia[16].SetActive(true);
                SoundManager.Instance.PlaySE("fuss");
            }
            else if (content == "Sound") {
                EventNum = 2;
                SoundManager.Instance.PlaySE("WindowKnock", 2f);
            }
            else if (content == "Next")
            {
                content = null;
                EventNum = 0;
            }
            else if (content == "BlinkOpen_Effect")
            {
                StartCoroutine(EffectEnd(3.0f));
                Color tmp = Fade.color;
                tmp.a = 0;
                Fade.color = tmp;
                BA.BlinkOpen();
                EventNum = 15;
                content = null;
            }
            else if (content == "BlinkClose_Effect")
            {
                StartCoroutine(EffectEnd(3.0f));
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
            else if(content == "Scene3_Start")
            {
                EventNum = 14;
                content = null;
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
            else if (content == "BearMove")
            {
                if (diaGroupIndex == 743)
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
            else if (content == "PlayerMove")
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
            else if(content == "chooseOff")
            {
                isChoose = false;
            }
            else if(content == "knock")
            {
                gome.isFollow = false;
                SoundManager.Instance.PauseBGM();
                SoundManager.Instance.PlaySE("knock");
            }
            else if(content == "doorHandle")
            {
                SoundManager.Instance.PlaySE("handle");
            }
            else if (content == "SceneOver")
            {
                dia_p.diaScene3.SetActive(false);
                dia_p.diaScene4.SetActive(true);
                Vector3 targetPos = player.transform.position;
                targetPos.x -= 2.5f;
                gome.ChangeTarget(targetPos);
                block.enabled = true;
                //gome.isFollow = false;
                gome.isStart = true;
            }
            else if (content == "EyeMove_D")
            {
                ob[10].transform.DOLocalMoveX(13.9f, 1.0f);
                ob[11].transform.DOLocalMoveX(15.3f, 1.0f);
            }
            else if(content == "FrameFreeze")
            {
                EventNum = 13;
            }
            else if (content == "MiniGame")
            {
                gome.isMinigame = true;
                PlayerMove(4);
                isMini = true;
                gome.speed = 2.5f;
                content = null;
            }
            else if (content == "Eight")
            {
                PlayerPrefs.SetInt("savePoint", 2);
                gome.isMinigame = true;
                PlayerMove(4);
                ob[13].SetActive(true);
                isMini = true;
                gome.speed = 2.5f;
                EventNum = 8;
                content = null;
            }
            else if (content == "LightOn")
            {
                SoundManager.Instance.PauseBGM();
                SoundManager.Instance.PlaySE("LightOnOff");
                ob[12].SetActive(true);
                gome.isStart = false;
                gome.isFollow = false;
                content = null;
            }
            else if (content == "Hide")
            {
                SoundManager.Instance.PlaySE("fuss");
                EventNum = 10;
                content = null;
            }
            else if (content == "Surprise")
            {
                SoundManager.Instance.PlaySE("surprise");
                surprise.enabled = true;
                content = null;
                EventNum = 7;
            }
            else if(content == "wakeup")
            {
                EventNum = 17;
            }
            else if (content == "Choose")
            {
                fadeinout.Blackout_Func(0.7f);
                gome.transform.position = tPos[5].transform.position;
                gome.ChangeTarget(tPos[5].transform.position);
                player.transform.eulerAngles = new Vector3(0, 180, 0);
                PlayerMove(1);
                content = null;
                PlayerPrefs.SetInt("savePoint", 1);
                EventNum = 13;
                Dia[37].SetActive(true);
                Dia[38].SetActive(true);
                Dia[39].SetActive(true);
            }
            else if (content == "Gome")
            {
                Vector3 target = player.transform.position;
                target.x -= 3;
                gome.ChangeTarget(target);
                gome.isFollow = false;
                gome.isStart = true;
            }
            else if (content == "LightBlink1")
            {
                StartCoroutine(LBlink(1));
            }
            else if (content == "LightBlink2")
            {
                StartCoroutine(LBlink(2));
            }
            else if(content == "bookStack")
            {
                SoundManager.Instance.PlaySE("bookStack");
                ob[16].SetActive(false);
                ob[17].SetActive(true);
            }
            else if(content == "chair")
            {
                SoundManager.Instance.PlaySE("chair");
                ob[2].transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if(content == "closet")
            {
                SoundManager.Instance.PlaySE("closet");
                ob[18].SetActive(true);
            }
            else if (content == "GameOver")
            {
                SoundManager.Instance.PauseBGM();
                EventNum = 16;
                content = null;
            }
            else if (content == "Ending")
            {
                fadeinout.Blackout_Func(0.3f);
                StartCoroutine(Ending());
            }

            content = null;
        }
    }

    IEnumerator chooseBlink()
    {
        isBlink = true;
        Color tmp = choose[0].color;
        float alpha;

        if (tmp.a > 0)
            alpha = 0;
        else
            alpha = 1;

        choose[0].DOFade(alpha, 0.3f);
        choose[1].DOFade(alpha, 0.3f);

        yield return new WaitForSeconds(0.3f);
        isBlink = false;
    }

    public void Outline_false()
    {
        DiaInterInfo Line;
        Line = Dia[outline - next_true].transform.GetComponent<DiaInterInfo>();
        Line.outline_off();
    }

    IEnumerator EffectEnd(float durTime)
    {
        yield return new WaitForSeconds(durTime);
        dp.CurrentDiaIndex = effectIndex;
        TM.DiaUI.SetActive(true);
    }

    IEnumerator LBlink(int times)
    {
        Dialogue_system_manager.GetComponent<TextManager>().Increasediaindex = false;
        for (int i = 0; i < times; i++)
        {
            Color tmp = Fade.color;
            tmp.a = 0.7f;
            Fade.color = tmp;
            SoundManager.Instance.PlaySE("LightOnOff", 0.7f);
            yield return new WaitForSeconds(0.3f);
            tmp.a = 0;
            Fade.color = tmp;
            SoundManager.Instance.PlaySE("LightOnOff", 0.7f);
            yield return new WaitForSeconds(0.3f);
        }
        Dialogue_system_manager.GetComponent<TextManager>().Increasediaindex = true;
    }

    IEnumerator Bigger()
    {
        SoundManager.Instance.PlaySE("bigGome");
        ob[5].transform.DOScaleX(-2.3f, 0.5f);
        ob[5].transform.DOScaleY(2.3f, 0.5f);
        yield return new WaitForSeconds(0.2f);
        Dialogue_system_manager.GetComponent<TextManager>().Increasediaindex = true;
    }

    IEnumerator Ending()
    {
        ending_bg.enabled = true;
        eye.enabled = true;
        SoundManager.Instance.PlaySE("gomeEyes");
        yield return new WaitForSeconds(0.5f);
        eye.transform.DOLocalMoveX(-138, 2.5f);
        yield return new WaitForSeconds(4.0f);
        fadeinout.Fade_In();
        SceneChanger.Instance.ChangeScene(SceneType.Diary);
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
        if (isUnperform)
        {
            ob[14].SetActive(true);
            isUnperform = false;
        }
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

    public void unperform()
    {
        if (!dp.Complete_Condition.Contains(750) || !dp.Complete_Condition.Contains(751))
        {
            SoundManager.Instance.PauseBGM();
            isUnperform = true;
            TM.DiaUI.SetActive(false);
            //플레이어 멈추고
            player.isStop = true;

            //고미 멈추고
            gome.isStart = false;
            gome.isMinigame = false;
            gome.isFollow = false;

            //고미 달려온다
            gome.speed = 7.0f;
            Vector3 targetPos = player.targetPos;
            targetPos += new Vector3(1.5f, 0, 0);
            gome.ChangeTarget(targetPos);
            gome.isStart = true;
        }
        else
        {
            next_true = 707;
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
        SoundManager.Instance.PlaySE("GameOver");
        yield return new WaitForSeconds(1.5f);
        Color tmp = gameOver.color;
        tmp.a = 1;
        gameOver.color = tmp;
        gameOver.DOText(gameOver.text, 0.5f);
        yield return new WaitForSeconds(0.5f);

        PlayerPrefs.SetInt("isGameOver", 1);
        SceneChanger.Instance.RestartScene();


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
