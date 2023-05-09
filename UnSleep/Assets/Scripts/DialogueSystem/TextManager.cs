using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TextManager : MonoBehaviour
{
    [Header("UI 위치")]
    public GameObject DiaUI; //대화UI
    public GameObject SelectUI; //선택지UI
    public GameObject LogUI; //로그UI


    //<---------- 대화 UI---------------->
    [Header("대화 UI")]
    public GameObject NAMETAG;
    public Text NameText;
    public Text LineText;
    private bool isTyping;
    private Coroutine type_coroutine;

    public Image Speaker1; //왼쪽 초상화
    public Image Speaker2; //오른쪽 초상화

    //<---------- 선택지 UI---------------->
    [Header("선택지 UI")]
    public Text TextA;
    public Text TextB;
    public Button ButtonA;
    public Button ButtonB;
    public bool SelectA = false;
    private bool loading = false;

    //<----------잘 있어요 퍼즐 UI-------------->
    [Header("잘 있어요 퍼즐 UI")]
    public PuzzleClear puzzleClear;
    public GameObject goodbyeUI;
    public Image goodbyeImg;
    public Text goodbyeText;
    public bool isGoodbye = false;


    //<---------- 배경---------------->
    [Header("배경")]
    public Image BackGround;
    public Image Change_BackGround;


    private bool BG_Change;
    private SpriteManager spriteManager;


    //<---------- 기타 정보 ---------------->
    [Header("기타 정보")]
    public bool isDnI;
    public EffectManager effectManager;

    //<---------- 기타 정보 ---------------->

    public int Dia_Id;
    private Dialogue_Proceeder dp;
    private bool showSpeaker2When1 = false; //발화자 1일때 2를 회색으로 출력할 것인지
    private bool showSpeaker1When2 = false; //발화자 2일때 1을 회색으로 출력할 것인지


    [Tooltip("클릭 상호작용시 처음 인덱스가 넘어가버리는 현상 방지. true일때만 대화 진행")]
    public bool Increasediaindex = true; //클릭 상호작용시 처음 인덱스가 넘어가버리는 현상 방지. true일때만 대화 진행



    [Header("씬 전환")]
    public SceneTransEffectManager STEManager; //씬 전환 효과
    public GameObject UI_Objects; //대화UI 레이아웃 변경 주관하는 녀석

    [Header("대화 종료 구독자들")]
    private List<DialogueDoneListener> dialogueDoneListeners = new List<DialogueDoneListener>();


    [SerializeField] public Dictionary<int, DialogueEvent> DiaDic = new Dictionary<int, DialogueEvent>(); //대화묶음 딕셔너리

    [SerializeField] private Dictionary<int, Sprite[]> PorDic = new Dictionary<int, Sprite[]>(); //초상화 딕셔너리

    Dictionary<int, string> NAMEDic = new Dictionary<int, string>() //이름 딕셔너리. 캐릭터 id가 있는 주요 인물들
    {
        {1000,"잠재우미" },{1001,"도문"}, {1002,"어머니"}, {1003, "아버지"},
        {1004, "재준"}, {1005, "장현"}, {1006, "새나"}, {1007, "이비"},
        {1008, "구광일"}, {1009, "고준일"}
    };

    private int[] cha_ids; //매 에피소드 당 등장하는 인물들 id를 담은 배열.
    //csv파일 최상단에 입력하도록 할 계획

    //7세
    public bool isSeven;
    public string con;
    public bool isEnd;
    public MovieEffect movie;
    public int movie_cnt;
    public DiaPlayer dia_p;
    public DiaEvent diaEvent;

    public bool isNoise;
    public NoiseManager NM;
    public TugOfWar TW;
    public ObManager OM;
    public bool isMovieIn;
    public bool isMovieOut;
    public bool isReplay;

    public Gome gome;
    public bool EffectEnd = true;

    void Awake()
    {
        // Dialogue_Proceeder 사용을 편리하게 하기 위해
        dp = Dialogue_Proceeder.instance;

        // Background 조절을 위한 매니저 함수
        spriteManager = GetComponent<SpriteManager>();


        // 파싱되어 배열로 받는다
        DialogueParser dialogueParser = GetComponent<DialogueParser>();
        var dialogueEventList = dialogueParser.Parse_Dialogue();
        foreach (var item in dialogueEventList)
        {
            // (대화묶음id , 대화묶음) 꼴로 묶는다
            DiaDic[item.DiaKey] = item;
        }

        // 매 에피소드 당 등장하는 인물들 id를 담은 배열
        cha_ids = dialogueParser.GetCharId();

        // 18세 에피소드 기준 cha_ids에 담긴 인물들의 초상화가 담긴 딕셔너리를 받는다
        Portrait portrait = GetComponent<Portrait>();
        PorDic = portrait.GetPortraitDic(cha_ids, dp.CurrentEpiID);
    }


    private void Start()
    {

        //씬 시작 시 Dialogue_Proceeder에게서 정보 받아온다
        Dia_Id = dp.CurrentDiaID; //현재 대화 묶음 id

        if (dp.Satisfy_Condition(DiaDic[Dia_Id].Condition))
            Set_Dialogue_System();
        else
        {
            DiaUI.SetActive(false);
            goodbyeUI.SetActive(false);

        }

        //배경 전환
        if (DiaDic[Dia_Id].dialogues[0].BG != null)
            Change_IMG(BackGround, Change_BackGround, DiaDic[Dia_Id].dialogues[0].BG);


        if (DiaDic[Dia_Id].SceneNum == 1 && DiaDic[Dia_Id].BGM != null)
            SoundManager.Instance.PlayBGM(DiaDic[Dia_Id].BGM);




        if (!DiaDic.ContainsKey(Dia_Id - 1)) //처음 시작 시
        {
            if (!isNoise && !isSeven && STEManager != null)
                STEManager.WaitBlackOut(2f); //매개변수 만큼 암막 상태로 대기했다가 밝아집니다
        }
        else //에피소드 중간에 씬 전환 후 첫 시작
        {
            //스토리 -> 정신세계 전환 시 시네마틱 인트로 재생
            if (DiaDic[Dia_Id - 1].SceneNum == 1 && DiaDic[Dia_Id].SceneNum == 2)
            {
                GameObject.Find("Cinematic").transform.GetChild(2).gameObject.SetActive(true);
            }
            //정신세계 -> 스토리 전환 시 눈뜨면서 시작

            else if (DiaDic[Dia_Id - 1].SceneNum == 2 && DiaDic[Dia_Id].SceneNum == 1)
            {
                STEManager.BlinkOpen();
            }
        }

    }


    public void addDialogueDoneListeners(DialogueDoneListener listener)
    {
        dialogueDoneListeners.Add(listener);
    }

    void Update()
    {

        //얘 다시 살렸음 (6세 연출땜에)
        if (Dia_Id != dp.CurrentDiaID)
            Dia_Id = dp.CurrentDiaID;



        // Complete_Condition = dp.Dia_Complete_Condition; //움,, 이거 왜 있지? 빼면 무서우니까 일단 주석으로
        //-> 이거 빼니까 정신세계 맵에서 상호작용으로 갱신된 완료 조건에 TextManager에 반영이 안되네요

        // if (DiaUI.activeSelf == false && (DiaDic[Dia_Id].Condition.Equals(Complete_Condition) || DiaDic[Dia_Id].Condition.Equals("")))
        //    DiaUI.SetActive(true); //대화 조건 새로 충족하면 대화 활성화: 대화 조건이 공란이면 조건x 무조건 실행
        //-> 조건에 맞아야 대화 발생


        //스토리 -> 대화 발생 조건 충족 -> 바로 활성화. 이부분은 주로 선택지 -> 대화로 돌아올 때 실행될 것.
        if (DiaDic[Dia_Id].SceneNum == 1 && DiaUI.activeSelf == false)
        {
            if ((DiaDic[Dia_Id].Condition.Length == 1 && DiaDic[Dia_Id].Condition[0] == 0) || dp.Satisfy_Condition(DiaDic[Dia_Id].Condition))
            {
                DiaUI.SetActive(true);
            }

        }



        //클릭시에 1. Log가 꺼져있고 2. 대화UI가 켜져있고 3.Raycast Target이 false인 UI 위일 때 (배경, 대사 창)
        if (Input.GetMouseButtonDown(0) && Increasediaindex)
        {
          


            if (goodbyeUI.activeSelf && !isGoodbye && dp.CurrentEpiID == 19)
            {

                if (dp.CurrentDiaIndex < DiaDic[Dia_Id].dialogues.Length - 1) //대화 묶음 내에서 다음 대사로 접근
                {
                    dp.CurrentDiaIndex++;
                    StartCoroutine(Update_Dialogue_Goodbye());
                }
                else
                {
                    dp.AddCompleteCondition(Dia_Id);
                    dp.UpdateCurrentDiaID(Dia_Id);
                    Set_Off_Dialogue_Goodbye();
                    dp.CurrentDiaIndex = 0;
                }

            }
            else if (DiaUI.activeSelf && !LogUI.activeSelf && !EventSystem.current.IsPointerOverGameObject())
            {
               
                if (isTyping)
                {
                    if (type_coroutine != null)
                    {
                        StopCoroutine(type_coroutine);
                        LineText.text = DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].contexts;
                        isTyping = false;
                        StartCoroutine(debounce()); // 강제 넘김 시 잠깐의 딜레이
                    }
                }
                else
                {
                    if(isNoise)
                        Get_Content();

                    if (dp.CurrentDiaIndex < DiaDic[Dia_Id].dialogues.Length - 1) //대화 묶음 내에서 다음 대사로 접근
                    {


                        if (DiaUI.activeSelf && Increasediaindex) //대화 UI가 켜져 있고, 연출등의 이유로 인덱스 변화를 막지 않은 경우에 대화진행
                        {
                            dp.CurrentDiaIndex++;
                        }
                            

                        
                        if (isSeven)
                        {
                            con = DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].Content;
                            if (con != null)
                            {
                                if(con.IndexOf("Effect") != -1)
                                {
                                    diaEvent.effectIndex = dp.CurrentDiaIndex;
                                    EffectEnd = false;
                                    DiaUI.SetActive(false);
                                }

                                diaEvent.content = con;
                            }
                        }
                        

                        if (DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].isSelect) //선택지인 경우
                            Set_Select_System();
                        else
                            Set_Dialogue_System();
                            


                        //배경 전환
                        if (DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].BG != null && !loading)
                            Change_IMG(BackGround, Change_BackGround, DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].BG);


                    }
                    else //대화 묶음 넘어갈 때
                    {


                        dp.AddCompleteCondition(Dia_Id); //대화 종료. 완수 조건에 현재 대화묶음id 추가
                        foreach (DialogueDoneListener listener in dialogueDoneListeners)
                        {
                            listener.OnDialogueEnd(Dia_Id);
                        }
                        

                        if (!DiaDic.ContainsKey(Dia_Id + 1)) //다음 대사가 없으면
                        {
                            dp.End = true; // 끝났음 true. 일기장에서 보고 자동 페이지 넘김과 후일담 출력
                            dp.CurrentDiaIndex = 0;
                            Increasediaindex = false;
                            SaveDataManager.Instance.SaveEpiProgress(dp.CurrentEpiID + 1); //현재 에피소드 완료 저장
                            SceneChanger.Instance.ChangeScene(SceneType.Diary);
                        }

                        if (DiaDic.ContainsKey(Dia_Id + 1) && DiaDic[Dia_Id].SceneNum == DiaDic[Dia_Id + 1].SceneNum) //씬 변화가 없음
                        {
                            dp.AddCompleteCondition(Dia_Id); //대화 종료. 완수 조건에 현재 대화묶음id 추가



                            //if (Increasediaindex && !isSeven && STEManager != null && DiaDic[Dia_Id].SceneNum == 1)
                            //    STEManager.FadeInOut();


                            //대화 묶음 넘어갈 때 초상화 초기화
                            showSpeaker2When1 = false;
                            showSpeaker1When2 = false;


                            if (DiaDic[Dia_Id].SceneNum == 1) //스토리모드
                            {
                                if (SelectA) //선택지 2개 기준. A를 누르면 대화 묶음 하나 더 넘어가도록
                                {
                                    //ex. 선택지A결과(1811) 선택지B결과(1812) 다음대화(1813)일 때 1811에서 바로 1813으로 넘어가도록.
                                    //선택지 개수를 동적으로 바꾼다면 수정해야 함
                                    if (dp.CurrentEpiID == 12) //어라.. 선택지 후 답이 똑같네 예외처리띠~
                                        Dia_Id++;
                                    else
                                        Dia_Id += 2;
                                    dp.UpdateCurrentDiaID(Dia_Id);
                                    SelectA = false;
                                }
                                else
                                {
                                    if (dp.Satisfy_Condition(DiaDic[Dia_Id + 1].Condition)) //씬 변경 없이 다음 대화묶음의 조건이 완수된 경우 바로 이동 (평상시)
                                    {
                                        
                                        dp.CurrentDiaIndex = 0; //대사 인덱스 초기화
                                        Dia_Id += 1; //다음 대화 묶음으로


                                        dp.UpdateCurrentDiaID(Dia_Id); //Proceeder 업데이트.


                                        //BGM 전환
                                        if (DiaDic[Dia_Id].BGM != null && DiaDic[Dia_Id].SceneNum == 1)
                                        {
                                            if (DiaDic[Dia_Id].BGM.Equals("stop"))
                                                SoundManager.Instance.FadeOutBGM();
                                            else
                                                SoundManager.Instance.PlayBGM(DiaDic[Dia_Id].BGM);
                                        }


                                    }
                                    else //연출 등의 이유로 잠시 대화를 멈췄다가 재개하는 경우
                                    {
                                        Increasediaindex = false;
                                    }
                                }
                            }
                            else if (DiaDic[Dia_Id].SceneNum == 8)
                            {
                                // Nightmare 씬에서 대화 묶음 끝났을 때 처리가 필요하면 여기에 
                                // 아마 층간소음에서는 대화 종료 후 현재 상황에 따라서 미니게임 진행 등을 시작하면 될 것

                                if (con == "GameStart_1")
                                {
                                    NM.DM.GameSetting(true);
                                    con = null;
                                }
                                else if (con == "GameStart_2")
                                {
                                    if (!NM.isStart)
                                        NM.coStart(2, 3);
                                }
                                else if(con == "MonsterMove")
                                {
                                    Debug.Log("MonsterMove");
                                    OM.isFull = true;
                                }
                                else if (con == "GameStart_3_1")
                                {
                                    TW.isMouseMove = true;
                                }
                                else if (con == "GameStart_3_2")
                                {
                                    TW.TW.raycastTarget = true;
                                    TW.isStart = true;
                                }
                                else if(con == "End")
                                {
                                    SceneChanger.Instance.ChangeScene(SceneType.Diary);
                                }


                                // 아래 코드는 테이블 확인용 임시 코드.
                                // 그냥 원래 스토리에서 진행되듯 넘어가는 코드입니다.
                                // 층간 작업하실 때 지우고 쓰시면 됨.

                                if (isReplay)
                                {
                                    Debug.Log("Dia: " + Dia_Id);
                                    Dialogue_Proceeder.instance.RemoveCompleteCondition(Dia_Id);
                                    isReplay = false;
                                }

                                Dia_Id++;
                                dp.UpdateCurrentDiaID(Dia_Id);
                                Increasediaindex = false;
                                DiaUI.SetActive(false); //대화가 끝나면 대화 UI 끄기.
                                dp.CurrentDiaIndex = 0; //대사 인덱스 초기화
                            }
                            else //맵모드 + 동굴 + 7세까지 처리. 머지할때 잘 보고 하기
                            {
                                if (SelectA) //선택지 2개 기준. A를 누르면 대화 묶음 하나 더 넘어가도록
                                {
                                    //ex. 선택지A결과(1811) 선택지B결과(1812) 다음대화(1813)일 때 1811에서 바로 1813으로 넘어가도록.
                                    //선택지 개수를 동적으로 바꾼다면 수정해야 함
                                    int before = Dia_Id;
                                    if (dp.CurrentEpiID == 12) //어라.. 선택지 후 답이 똑같네 예외처리띠~
                                        Dia_Id++;
                                    else
                                        Dia_Id += 2;
                                    // dp.UpdateCurrentDiaID(Dia_Id);
                                    
                                    proceedScene(before, Dia_Id);
                                    // SelectA = false;
                                }

                                Increasediaindex = false;
                                DiaUI.SetActive(false); //대화가 끝나면 대화 UI 끄기.
                            }



                            if (isSeven)
                            {
                                Debug.Log("isEnd");
                                isEnd = true;
                                Player.instance.isStop = false;
                                Player.instance.col.enabled = true;
                                SpriteOutline.instance.isStop = false;
                                if (diaEvent.outline != 0)
                                    diaEvent.Outline_false();

                                if (isMovieOut)
                                {
                                    diaEvent.isMovie = true;
                                    isMovieIn = false;
                                    isMovieOut = false;
                                }

                                gome.isStart = true;
                                if (gome.isFollow)
                                    SoundManager.Instance.PlayBGM("gomeFollow");
                            }


                            if (DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].isSelect) //선택지인 경우
                                Set_Select_System();
                            else {
                                if (!loading)
                                    Set_Dialogue_System();
                            }
                              


                            //배경 전환
                            if (DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].BG != null && !loading)
                                Change_IMG(BackGround, Change_BackGround, DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].BG);
                        }
                        else //씬 변화가 있음
                        {
                            if (DiaDic[Dia_Id].SceneNum != 7) // 동굴은 동굴에서 처리
                                proceedScene(Dia_Id, Dia_Id + 1);
                        }
                    }
                }
            }


        }

    }

    private void proceedScene(int beforeId, int nextId)
    {
        dp.CurrentDiaIndex = 0; //대사 인덱스 초기화
        if (DiaDic[beforeId].SceneNum == 1 && DiaDic[nextId].SceneNum == 2) //스토리->정신세계
        {
            SoundManager.Instance.FadeOutBGM();
            StartCoroutine(LoadStoryMental(SceneType.Mental));
        }
        else if (DiaDic[beforeId].SceneNum == 2 && DiaDic[nextId].SceneNum == 1) //정신세계(퍼즐)->스토리
        {
            SoundManager.Instance.FadeOutBGM();
            StartCoroutine(LoadStoryMental(SceneType.Dialogue));
        }
        else if (DiaDic[beforeId].SceneNum == 1 && DiaDic[nextId].SceneNum == 9)
        {
            StartCoroutine(LoadStoryMental(SceneType.Nightmare27));
        }
        else // 그 밖의 경우에는 단순 대화 종료. (ex) 스토리 맵 -> 동굴 이동 전 대기 상태
        {

            dp.AddCompleteCondition(beforeId); //대화 종료. 완수 조건에 현재 대화묶음id 추가

            DiaUI.SetActive(false); //대화가 끝나면 대화 UI 끄기.
        }
    }

    public void Get_Content()
    {
        if (isSeven || isNoise)
        {
            con = DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].Content;
            if (con != null)
            {
                if (isSeven)
                {
                    if (con.IndexOf("Effect") != -1)
                    {
                        Debug.Log("Effect");
                        diaEvent.effectIndex = dp.CurrentDiaIndex;
                        EffectEnd = false;
                        DiaUI.SetActive(false);
                    }

                    diaEvent.content = con;
                }
                else
                    NoiseManager.instance.con = con;
            }
            Debug.Log("con3: " + con);
        }
    }


    public void Set_Select_System()
    {
        SelectUI.SetActive(true);
        DiaUI.SetActive(false);

        dp.AddCompleteCondition(Dia_Id); //대화 종료. 완수 조건에 현재 대화묶음id 추가

        TextA.text = DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].contexts;
        TextB.text = DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex + 1].contexts;

        //선택지 누르면 나올 다음 대사 묶음 id
        int nextA = DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].nextDiaKey;
        int nextB = DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex + 1].nextDiaKey;

        //버튼 누를 때 인자 전달하려고 이렇게 작성
        ButtonA.onClick.AddListener(delegate () { Select(nextA, true); });
        ButtonB.onClick.AddListener(delegate () { Select(nextB, false); });

        //if (DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].background != -1)
        //    BackGround.sprite = BG[DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].background];



        dp.UpdateCurrentDiaID(Dia_Id); //선택지 선택으로 변한 Dia_Id Proceeder 업데이트.



    }

    public void Set_Dialogue_System()
    {
        Dialogue dialogue = DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex];
        string NAME = dialogue.name; //이름
        string CONTEXT = dialogue.contexts; //대사
        int EMOTION = dialogue.portrait_emotion; //초상화id (표정id)
        float result; //이름(문자열)이 문자인지 숫자인지 판단하기 위해 있는 변수 (아래 보면 알아요)
        int LAYOUT = dialogue.layoutchange; //레이아웃 변화
        string CONTENT = dialogue.Content;//상호작용명(int가 될 수 있음)
        string SE = dialogue.SE; //효과음
        //string BGM = DiaDic[Dia_Id].BGM; // 배경음악 (스토리)

        UI_Objects.GetComponent<ChangeLayout>().LayoutChange(LAYOUT); //전달. 저쪽에서 알아서 할거임

        if (LAYOUT == 5) //잘 있어요 전용
        {
            Set_Dialogue_Goodbye();
            DiaUI.SetActive(false);
            return;
        }



        if (LAYOUT == 3 && !isDnI)
        {
            GetComponent<Run_DnI>().Run_Direc_N_Inter();
            isDnI = true;
            Increasediaindex = false;
        }


        if (LAYOUT == 7)
        {
            Increasediaindex = false;
        }


        if (SE != null) //효과음 있으면 효과음 재생
        {
            if (SE.Equals("stop"))
                SoundManager.Instance.StopSE();
            else
                SoundManager.Instance.PlaySE(SE);

        }

     
        if (DiaDic[Dia_Id].SceneNum == 2)
        {
            if (!string.IsNullOrEmpty(CONTENT) && effectManager != null)
            {
                effectManager.OnEffect();
            }
        }
        /*
        if (BGM != null)
        {
            //SoundManager.Instance.PlayBGM(BGM);

        }
        */

        if (NAME.Equals(string.Empty)) //나레이션 -> 이름, 초상화 Off
        {
            NAMETAG.SetActive(false);
            Speaker1.color = new Color(1f, 1f, 1f, 0f); //투명하게 처리해서 없는 것처럼
            Speaker2.color = new Color(1f, 1f, 1f, 0f);

        }
        else //나레이션 아닐 때
        {
            NAMETAG.SetActive(true); //이름 키고

            if (float.TryParse(NAME, out result)) //이름이 숫자면 = 이름이 캐릭터id면
                NameText.text = NAMEDic[int.Parse(NAME.ToString())]; //이름 딕셔너리에서 가져오기
            else //이름이 문자면 -> 엑스트라
                NameText.text = NAME; //표기된 그대로 출력


            if (DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].portrait_position == 0) //독백
            {
                Speaker1.color = new Color(1f, 1f, 1f, 1f); //좌측 보이게

                if (float.TryParse(NAME, out result)) //캐릭터 id
                    Speaker1.sprite = PorDic[int.Parse(NAME.ToString())][EMOTION]; //초상화 딕셔너리에서 맞는 표정 가져오기
                else //엑스트라
                    Speaker1.sprite = PorDic[9999][EMOTION];

                showSpeaker1When2 = true; //스피커1에 이미지가 들어있으므로 회색처리해도 됨
                Speaker2.color = new Color(0f, 0f, 0f, 0f); //우측 투명하게

            }
            else if (DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].portrait_position == 1) //왼쪽
            {
                Speaker1.color = new Color(1f, 1f, 1f, 1f); //좌측 보이게

                if (float.TryParse(NAME, out result))
                    Speaker1.sprite = PorDic[int.Parse(NAME.ToString())][EMOTION];
                else
                    Speaker1.sprite = PorDic[9999][EMOTION];

                showSpeaker1When2 = true; //스피커1에 이미지가 들어있으므로 회색처리해도 됨

                if (!showSpeaker2When1) //처음 나오는 발화자 id 1이면 2에 듣는 상대가 존재x
                    Speaker2.color = new Color(0.5f, 0.5f, 0.5f, 0f); //우측 안보이게
                else
                    Speaker2.color = new Color(0.5f, 0.5f, 0.5f, 1f); //우측 회색으로

            }
            else if (DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].portrait_position == 2) //오른쪽
            {
                Speaker2.color = new Color(1f, 1f, 1f, 1f); //우측 보이게
                if (float.TryParse(NAME, out result)) //주요인물이면 +n 하여 우측 이미지로 접근. n은 emotion_cnt로 리턴 받음. Portrait 스크립트도 참고
                    Speaker2.sprite = PorDic[int.Parse(NAME.ToString())][EMOTION + emotion_cnt(int.Parse(NAME.ToString()))];
                else
                    Speaker2.sprite = PorDic[9999][EMOTION + 25]; //엑스트라에 더해지는 값은 엑스트라 이미지 총 개수. 늘어날때마다 수정해주기


                showSpeaker2When1 = true; //스피커2에 이미지가 들어있으므로 회색처리해도 됨


                if (Speaker1.sprite == null || !showSpeaker1When2) //현재 대화묶음 기준 왼쪽 발화자가 없는 경우에는 표시하지 않음.
                    Speaker1.color = new Color(0.5f, 0.5f, 0.5f, 0f);
                else
                    Speaker1.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }

        }

        //이전 대사 출력 효과 코루틴을 멈춘 후 비우고 새로운 대사 시작 (220403수정)
        if (type_coroutine != null)
            StopCoroutine(type_coroutine);
        LineText.text = "";

        if (DiaDic[Dia_Id].SceneNum == 1)
        {
            float delay = (dp.CurrentDiaIndex == 0) ? 1.2f : 0f;
            type_coroutine = StartCoroutine(OnType(0.03f, CONTEXT, delay));
        }
        else
        {
            type_coroutine = StartCoroutine(OnType(0.03f, CONTEXT));

        }
    }

    public void CombackfromDnI()
    {
        if (dp.CurrentDiaIndex < DiaDic[Dia_Id].dialogues_size - 1) //대화묶음 중간에 상호작용을 한 경우
            dp.CurrentDiaIndex++;
        else //상호작용을 한 후에 대화묶음이 넘어가는 경우
        {
            dp.CurrentDiaIndex = 0;
            Dia_Id++;
            dp.UpdateCurrentDiaID(Dia_Id);
        }

        isDnI = false;
        Increasediaindex = true;
        Set_Dialogue_System();
    }


    void Select(int nextDiaKey, bool isA) //선택지 선택
    {
        Dia_Id = nextDiaKey; //다음 대화 묶음 id
        dp.CurrentDiaIndex = 0; //대사 idx 초기화를 먼저 해야 갱신이 됨
        Set_Dialogue_System();

        dp.UpdateCurrentDiaID(nextDiaKey);
        SelectUI.SetActive(false); //선택지 UI끄고 대화 UI키기
        DiaUI.SetActive(true);
        SelectA = isA; //A를 눌렀으면 true, Dia_Id 하나 더 올리는 flag
    }

    public int emotion_cnt(int cha_id) //우측 초상화 가져오기 위한 단위 - 표정 개수
    {
        if (cha_id == 1002 || cha_id == 1003 || cha_id == 1008 || cha_id == 1009) //어머니 아버지 구광일 고준일
            return 3;
        else if (cha_id >= 1000 && cha_id <= 1007) //잠재우미 도문 재준 장현 새나 이비
            return 10;
        else //엑스트라
            return 1;
    }

    void Change_IMG(Image target, Image Change_target, string ImgName) //target: BackGround, Illust //Change_target: Change_BackGround, Change_Illust
    {

        if (ImgName.Equals("Transparent"))
        {
            target.color = new Color(1, 1, 1, 0);
            Change_target.color = new Color(1, 1, 1, 0);

            return;
        }

        Change_target.color = new Color(1, 1, 1, 0);
        spriteManager.LoadImage(Change_target, ImgName);
        //Change_BackGround.sprite = BG[BG_idx];

        if (!BG_Change)
        {

            StartCoroutine(fadein(target, Change_target, 0f, 1f, 0.5f, ImgName));

        }
    }


    public int[] ReturnDiaConditions(int id)
    {
        return DiaDic[id].Condition;
    }

    public void SetDiaInMap()
    {
        Dia_Id = dp.CurrentDiaID;
        dp.CurrentDiaIndex = 0;

        Set_Dialogue_System();
        if (DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].layoutchange != 5)
            DiaUI.SetActive(true);
        Get_Content();
        Increasediaindex = true;
    }



    private IEnumerator fadein(Image target, Image Change_target, float start, float end, float FadeTime, string ImgName)
    {

        BG_Change = true;
        float time = 0f;
        Color color = Change_target.color;
        color.a = Mathf.Lerp(start, end, time);

        while (color.a < 0.98f)
        {
            time += Time.deltaTime / FadeTime;
            color.a = Mathf.Lerp(start, end, time);
            Change_target.color = color;


            yield return null;

        }
        BG_Change = false;
        spriteManager.LoadImage(target, ImgName);
        Change_target.color = new Color(1, 1, 1, 0);

    }


    IEnumerator OnType(float interval, string Line, float delay = 0f)
    {
        yield return new WaitForSecondsRealtime(delay);

        isTyping = true;
        LineText.text = "";

        foreach (char item in Line)
        {
            LineText.text += item;
            yield return new WaitForSecondsRealtime(interval);
        }
        isTyping = false;
    }

    IEnumerator debounce()
    {
        Increasediaindex = false ;

        yield return new WaitForSecondsRealtime(0.2f);

        Increasediaindex = true;
    }


    //잘 있어요 퍼즐 전용 대화 UI
    public void Set_Dialogue_Goodbye() //처음 킬 때
    {

        DiaUI.SetActive(false);
        LogUI.SetActive(false);




        goodbyeUI.SetActive(true);
        StartCoroutine(OnGoodbyeImg());

        goodbyeText.text = DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].contexts;
        StartCoroutine(OnGoodbyeText());
    }

    IEnumerator Update_Dialogue_Goodbye() //한 대화 묶음 내에서 바꿀 때
    {

        yield return StartCoroutine(OffGoodbyeText()); //껐다가

        isGoodbye = true; //중간에 한번 꺼지니까 다시 막아

        goodbyeText.text = DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].contexts; //대사 갈아끼고


        StartCoroutine(OnGoodbyeText()); //킨다


        yield return null;
    }

    public void Set_Off_Dialogue_Goodbye()
    {
        Debug.Log("Set_Off_Dialogue_Goodbye");

        if (goodbyeUI.activeSelf)
        {
            StartCoroutine(OffGoodbyeText());
            StartCoroutine(OffGoodbyeImg());


            //퍼즐 끝났니?
            if (Dia_Id == 8024 || Dia_Id == 8026 || Dia_Id == 8030 || Dia_Id == 8034 || Dia_Id == 8039)
                puzzleClear.ClearPuzzle(SceneType.Mental, 1f); //땜빵으로 대사 넘길때까지 전 딜레이 해놧어요

            dp.UpdateCurrentDiaIDPlus1();
        }
    }

    IEnumerator OnGoodbyeImg()
    {
        isGoodbye = true;
        Color color = goodbyeImg.color;

        while (color.a < 0.98f)
        {
            color.a += 2f * Time.deltaTime;
            goodbyeImg.color = color;

            yield return null;
        }
        color.a = 1f;
        goodbyeImg.color = color;
        isGoodbye = false;
        yield return null;
    }

    IEnumerator OnGoodbyeText()
    {
        isGoodbye = true;
        Color color_t = goodbyeText.color;

        yield return new WaitForSeconds(0.5f); //대사 켤 때 찰나 대기
        while (color_t.a < 0.98f)
        {
            color_t.a += 2f * Time.deltaTime;
            goodbyeText.color = color_t;

            yield return null;
        }
        color_t.a = 1f;

        goodbyeText.color = color_t;
        isGoodbye = false;
        yield return null;
    }

    IEnumerator OffGoodbyeImg()
    {
        isGoodbye = true;
        Color color = goodbyeImg.color;

        while (color.a > 0.02f)
        {
            color.a -= 2f * Time.deltaTime;
            goodbyeImg.color = color;

            yield return null;
        }
        color.a = 0f;
        goodbyeImg.color = color;

        goodbyeUI.SetActive(false);
        isGoodbye = false;
        yield return null;
    }

    IEnumerator OffGoodbyeText()
    {
        isGoodbye = true;
        Color color_t = goodbyeText.color;

        while (color_t.a > 0.02f)
        {
            color_t.a -= 2f * Time.deltaTime;
            goodbyeText.color = color_t;

            yield return null;
        }
        color_t.a = 0f;
        goodbyeText.color = color_t;
        isGoodbye = false;

        yield return null;
    }


    IEnumerator LoadStoryMental(SceneType type)
    {
        Increasediaindex = false;
        loading = true;
        yield return new WaitForSeconds(1f);

        if (type == SceneType.Mental)
        {
            STEManager.BlinkClose();
            SoundManager.Instance.ChangeBGM("deepblue");
        }
        else if (type == SceneType.Dialogue || type == SceneType.Nightmare27)
            STEManager.FadeIn();

        yield return new WaitForSeconds(4f);



        dp.AddCompleteCondition(Dia_Id); //대화 종료. 완수 조건에 현재 대화묶음id 추가

        if (SelectA) dp.UpdateCurrentDiaID(Dia_Id + 2); //Proceeder 업데이트.
        else dp.UpdateCurrentDiaID(Dia_Id + 1); //Proceeder 업데이트.
        Increasediaindex = true;
        loading = false;
        SceneChanger.Instance.ChangeScene(type, false);
    }

    public Dictionary<int, DialogueEvent> getDiaDic()
    {
        return DiaDic;
    }

    public void StartLoadStoryMental()
    {
        if (dp.CurrentEpiID == 10)
        {
            StartCoroutine(LoadStoryMental(SceneType.Nightmare27));
            SoundManager.Instance.PlayBGM("creepy");
        }
        else
            StartCoroutine(LoadStoryMental(SceneType.Mental));
    }

    public Dictionary<int, Sprite[]> getPorDic()
    {
        return PorDic;
    }
}
