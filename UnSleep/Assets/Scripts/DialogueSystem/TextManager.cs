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
    private bool SelectA = false;


    //<---------- 로그 UI---------------->
    [Header("로그 UI")]
    public GameObject log_prefab;
    public GameObject Content;
    public GameObject BackToPopUp;
    public Button GoToPrevButton;
    public GameObject GoToPrev;
    public GameObject GoToCurr;
    public GameObject LogPanel;


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
                                                              //Complete_Condition = dp.Dia_Complete_Condition; //현재 완수한 대화 조건

        GoToPrevButton.interactable = false;

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

        if (DiaDic[Dia_Id].BGM != null)
            SoundManager.Instance.PlayBGM(DiaDic[Dia_Id].BGM);



        if (!DiaDic.ContainsKey(Dia_Id-1)) //처음 시작 시

        {
            if (!isSeven)
                STEManager.WaitBlackOut(3f); //매개변수 만큼 암막 상태로 대기했다가 밝아집니다
        }
        else //에피소드 중간에 씬 전환 후 첫 시작
        {
            //스토리 -> 정신세계 전환 시 시네마틱 인트로 재생
            if (DiaDic[Dia_Id - 1].SceneNum == 1 && DiaDic[Dia_Id].SceneNum == 2)
            {
                GameObject.Find("Cinematic").transform.GetChild(0).gameObject.SetActive(true);
            }
            //정신세계 -> 스토리 전환 시 눈뜨면서 시작

            else if (DiaDic[Dia_Id - 1].SceneNum == 2 && DiaDic[Dia_Id].SceneNum == 1)
            {
                STEManager.BlinkOpen();
            }
        }



    }


    void Update()
    {
        if (Dia_Id % 100 == 1)
            GoToPrevButton.interactable = false;
        else
            GoToPrevButton.interactable = true;


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
        if (Input.GetMouseButtonDown(0))
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
                    }
                }
                else
                {
                    if (dp.CurrentDiaIndex < DiaDic[Dia_Id].dialogues.Length - 1) //대화 묶음 내에서 다음 대사로 접근
                    {


                        if (DiaUI.activeSelf && Increasediaindex) //대화 UI가 켜져 있고, 연출등의 이유로 인덱스 변화를 막지 않은 경우에 대화진행
                            dp.CurrentDiaIndex++;

                        if (isSeven)
                        {
                            con = DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].Content;
                            Debug.Log("con1: " + con);
                        }


                        if (DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].isSelect) //선택지인 경우
                            Set_Select_System();
                        else
                            Set_Dialogue_System();


                        //배경 전환
                        if (DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].BG != null)
                            Change_IMG(BackGround, Change_BackGround, DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].BG);


                    }
                    else //대화 묶음 넘어갈 때
                    {


                        dp.AddCompleteCondition(Dia_Id); //대화 종료. 완수 조건에 현재 대화묶음id 추가

                        if (!DiaDic.ContainsKey(Dia_Id + 1)) //다음 대사가 없으면
                        {
                            dp.End = true; // 끝났음 true. 일기장에서 보고 자동 페이지 넘김과 후일담 출력
                            SaveDataManager.Instance.SaveEpiProgress(dp.CurrentEpiID+1); //현재 에피소드 완료 저장
                            SceneManager.LoadScene("Diary");
                        }

                        if (DiaDic[Dia_Id].SceneNum == DiaDic[Dia_Id + 1].SceneNum) //씬 변화가 없음
                        {
                            dp.AddCompleteCondition(Dia_Id); //대화 종료. 완수 조건에 현재 대화묶음id 추가



                            if (Increasediaindex && !isSeven && STEManager != null && DiaDic[Dia_Id].SceneNum == 1)
                                STEManager.FadeInOut();


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
                                          dp.CurrentDiaIndex = 0; //대사 인덱스 초기화화
                                          Dia_Id += 1; //다음 대화 묶음으로


                                          dp.UpdateCurrentDiaID(Dia_Id); //Proceeder 업데이트.

                                          //BGM 전환
                                          if (DiaDic[Dia_Id].BGM != null)
                                              SoundManager.Instance.PlayBGM(DiaDic[Dia_Id].BGM);


                                      }
                                      else //연출 등의 이유로 잠시 대화를 멈췄다가 재개하는 경우
                                      {
                                          Increasediaindex = false;
                                      }
                                  }
                              }
                            else //맵모드 + 동굴 + 7세까지 처리. 머지할때 잘 보고 하기
                            {

                                Increasediaindex = false;
                                DiaUI.SetActive(false); //대화가 끝나면 대화 UI 끄기.
                            }



                            /*
                            if (isSeven)
                            {
                                con = DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].Content;
                                Debug.Log("con2: " + con);
                            }
                            */

                            if (isSeven)
                            {
                                if (movie.isFramein)
                                    movie.MovieFrameout();

                                Debug.Log("isEnd");
                                isEnd = true;
                                Player.instance.isStop = false;
                                Player.instance.col.enabled = true;
                                Gome.instance.isStart = true;
                            }


                            if (DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].isSelect) //선택지인 경우
                                Set_Select_System();
                            else
                                Set_Dialogue_System();


                            //배경 전환
                            if (DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].BG != null)
                                Change_IMG(BackGround, Change_BackGround, DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].BG);
                        }

                        else //씬 변화가 있음
                        {
                            dp.CurrentDiaIndex = 0; //대사 인덱스 초기화
                            if (DiaDic[Dia_Id].SceneNum == 1 && DiaDic[Dia_Id + 1].SceneNum == 2) //스토리->정신세계
                            {
                                StartCoroutine(LoadStoryMental(SceneType.Mental));
                                //Dialogue_Proceeder.instance.UpdateCurrentDiaID(Dia_Id + 1); //Proceeder 업데이트.
                                //SceneManager.LoadScene("Mental_World_Map");
                            }
                            else if (DiaDic[Dia_Id].SceneNum == 2 && DiaDic[Dia_Id + 1].SceneNum == 1) //정신세계(퍼즐)->스토리
                            {

                                StartCoroutine(LoadStoryMental(SceneType.Dialogue));
                                //Dialogue_Proceeder.instance.UpdateCurrentDiaID(Dia_Id + 1); //Proceeder 업데이트.
                                //SceneManager.LoadScene("DialogueTest");


                            }
                            else // 그 밖의 경우에는 단순 대화 종료. (ex) 스토리 맵 -> 동굴 이동 전 대기 상태
                            {

                                dp.AddCompleteCondition(Dia_Id); //대화 종료. 완수 조건에 현재 대화묶음id 추가

                                DiaUI.SetActive(false); //대화가 끝나면 대화 UI 끄기.
                            }
                        }
                    }
                }
            }
        }

    }

    public void Get_Content()
    {
        if (isSeven)
        {
            con = DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].Content;
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
        string NAME = DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].name; //이름
        string CONTEXT = DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].contexts; //대사
        int EMOTION = DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].portrait_emotion; //초상화id (표정id)
        float result; //이름(문자열)이 문자인지 숫자인지 판단하기 위해 있는 변수 (아래 보면 알아요)
        int LAYOUT = DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].layoutchange; //레이아웃 변화
        string CONTENT = DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].Content;//상호작용명(int가 될 수 있음)
        string SE = DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].SE; //효과음

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
            Debug.Log("LayOut OUt");
            Increasediaindex = false;
        }


        if (SE != null) //효과음 있으면 효과음 재생
        {
            if (SE.Equals("stop"))
                SoundManager.Instance.StopSE();
            else
                SoundManager.Instance.PlaySE(SE);

        }


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
                    Speaker2.sprite = PorDic[9999][EMOTION + 21]; //엑스트라에 더해지는 값은 엑스트라 이미지 총 개수. 늘어날때마다 수정해주기


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
        type_coroutine = StartCoroutine(OnType(0.05f, CONTEXT));
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


    public void Log_On() //Log창 열기
    {
        LogUI.SetActive(true);

        LogPanel.GetComponent<LogAnimation>().Log_Open();
        Create_Log(Dia_Id, dp.CurrentDiaIndex); //Log 생성 -> 현재 대화묶음, 대사
    }

    public void Log_Off1() //Log창 끄기
    {
        LogPanel.GetComponent<LogAnimation>().Log_Close();

    }

    public void Log_Off2() //Log창 끄기
    {
        LogUI.SetActive(false);
        GoToCurr.SetActive(false);
        GoToPrev.SetActive(true);


        for (int i = 0; i < Content.transform.childCount; i++) //로그 프리팹 모두 삭제
        {
            Destroy(Content.transform.GetChild(i).gameObject);
        }

    }

    public void Show_Prev_Dia()
    {
        for (int i = 0; i < Content.transform.childCount; i++) //로그 프리팹 모두 삭제
        {
            Destroy(Content.transform.GetChild(i).gameObject);
        }

        Create_Log(Dia_Id - 1, DiaDic[Dia_Id - 1].dialogues_size); //이전 대화 묶음의 모든 대사를 묶음으로
        GoToCurr.SetActive(true);
        GoToPrev.SetActive(false);
    }

    public void Show_Curr_Dia() //이전 대화 봤다가 다시 돌아왔을 때
    {
        for (int i = 0; i < Content.transform.childCount; i++) //로그 프리팹 모두 삭제
        {
            Destroy(Content.transform.GetChild(i).gameObject);
        }

        Create_Log(Dia_Id, dp.CurrentDiaIndex); //Log 생성 -> 현재 대화묶음, 대사
        GoToCurr.SetActive(false);
        GoToPrev.SetActive(true);

    }

    void Create_Log(int diaId, int diaIndex) //Log 생성
    {

        for (int i = 0; i < diaIndex; i++) //현재 대화 묶음의 처음 ~ 직전 대사까지
        {
            int LAYOUT = DiaDic[diaId].dialogues[i].layoutchange;

            if (LAYOUT == 3) //3번 레이아웃(연출)은 로그에서 표시 안함. 연출로 인덱스가 꼬이는 것 방지
                continue;


            GameObject log = Instantiate(log_prefab); //프리팹 생성
            log.transform.SetParent(Content.transform); //스크롤 뷰 내에 "Content"의 자식들이 스크롤 뷰 리스트로 나타남

            log.name = "log_content";
            //이름 대사 초상화id 가져오고
            string NAME = DiaDic[diaId].dialogues[i].name;
            string CONTEXT = DiaDic[diaId].dialogues[i].contexts;
            int EMOTION = DiaDic[diaId].dialogues[i].portrait_emotion;
            float result; //이름(문자열)이 문자인지 숫자인지



            if (NAME.Equals("")) //나레이션이면
            {
                log.GetComponent<SetLogContent>().Set_narration(CONTEXT, diaId, i); //나레이션ver로 Set
            }
            else //대화면
            {
                Sprite char_img;
                if (float.TryParse(NAME, out result)) //캐릭터id면
                {
                    char_img = PorDic[int.Parse(NAME.ToString())][EMOTION]; //해당 초상화 가져와서
                    log.GetComponent<SetLogContent>().Set(char_img, NAMEDic[int.Parse(NAME.ToString())], CONTEXT, diaId, i); //정보 넘겨주면 set

                }
                else //엑스트라면
                {
                    char_img = PorDic[9999][EMOTION]; //해당 초상화 가져와서
                    log.GetComponent<SetLogContent>().Set(char_img, NAME, CONTEXT, diaId, i); //정보 넘겨주면 set

                }

            }
        }
    }

    public void BackToSeletedLogYes(int BackDiaid, int Backdialogidx)
    {
        if (BackDiaid < Dia_Id)
            dp.RemoveCompleteCondition(BackDiaid);
        Dia_Id = BackDiaid;
        dp.CurrentDiaIndex = Backdialogidx;
        dp.UpdateCurrentDiaID(BackDiaid);
        Set_Dialogue_System();



        //배경
        if (DiaDic[BackDiaid].dialogues[Backdialogidx].BG == null) //선택한 대사에 배경이 없으면
        {
            int j;
            for (j = Backdialogidx; j > 0; j--) //배경 이미지 있는 곳까지 올라가서
            {
                Debug.Log(BackDiaid.ToString() + " " + Backdialogidx.ToString());

                if (DiaDic[BackDiaid].dialogues[j].BG != null)
                {
                    Change_IMG(BackGround, Change_BackGround, DiaDic[BackDiaid].dialogues[j].BG);
                    break;
                }

            }

            Change_IMG(BackGround, Change_BackGround, DiaDic[BackDiaid].dialogues[0].BG); //바꾼다
        }
        else //선택한 대사에 배경이 있으면 그냥 바꿈
            Change_IMG(BackGround, Change_BackGround, DiaDic[BackDiaid].dialogues[Backdialogidx].BG);


        Log_Off1();
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
            //Debug.Log("배경바뀜");
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
        //Invoke("Set_Dialogue_System", 0.5f);
        Get_Content();
        Set_Dialogue_System();
        if (DiaDic[Dia_Id].dialogues[dp.CurrentDiaIndex].layoutchange != 5)
            DiaUI.SetActive(true);
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


    IEnumerator OnType(float interval, string Line)
    {
        isTyping = true;
        LineText.text = "";

        foreach (char item in Line)
        {
            LineText.text += item;
            yield return new WaitForSeconds(interval);
        }
        isTyping = false;
    }

    //잘 있어요 퍼즐 전용 대화 UI
    public void Set_Dialogue_Goodbye() //처음 킬 때
    {

        Debug.Log("Set_Dialogue_Goodbye");

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
        if (type == SceneType.Mental)
            STEManager.BlinkClose();
        else if (type == SceneType.Dialogue)
            STEManager.FadeIn();

        yield return new WaitForSeconds(4f);



        dp.AddCompleteCondition(Dia_Id); //대화 종료. 완수 조건에 현재 대화묶음id 추가
        dp.UpdateCurrentDiaID(Dia_Id + 1); //Proceeder 업데이트.
        SceneChanger.ChangeScene(type);
    }




}
