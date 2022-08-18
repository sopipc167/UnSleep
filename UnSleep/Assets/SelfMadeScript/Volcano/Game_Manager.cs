using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Manager : MonoBehaviour //게임의 전체적인 설정과 다른 오브젝트간의 상호작용을 관장하는 스크립트
{
    public int gameboardint; //현재 게임스테이지
    private GameObject GameBoard;
    protected class bombs //폭탄의 내용을 간략하게 저장하기 위한 구조체
    {
        Vector2 pos; //폭탄의 위치
        GameObject obj; //폭탄의 게임오브덱트
        public bombs(Vector2 v, GameObject g) 
        {
            pos = v;
            obj = g;
        }
        public GameObject getObj()
        {
            return obj;
        }
        public Vector2 getPos()
        {
            return pos;
        }
    }
    int [,,]gameboardArray=new int[6,12,16]
    {
        {
            { 0,-1,0,0,4,0,0,2,0,0,1,0,0,0,0,0},
          { 0,1,0,0,0,0,0,3,0,0,0,0,0,0,0,0},
          { 0,0,0,0,2,0,0,0,0,2,0,0,0,0,0,0},
          { 0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0},
          { 0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0},
          { 3,0,0,3,0,0,0,3,0,0,4,0,0,0,0,0},
          { 0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0},
          { 0,0,0,0,0,0,1,0,0,0,4,0,0,2,0,0},
          { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
          { 0,0,0,0,3,0,0,0,0,0,3,0,0,0,1,0},
          { 0,0,0,0,0,0,0,2,0,0,0,0,2,0,0,0},
          { 0,0,0,0,0,0,0,0,0,0,0,0,0,-1,0,0}
        },
        {
           { 0,0,0,0,0,0,-1,0,0,0,0,0,0,0,0,0},
           { 0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0},
           { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
              { 0,0,0,0,1,0,4,0,0,0,0,0,0,0,0,0},
              { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
              { 0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0},
              { 0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0},
              { 0,0,0,0,0,0,4,0,0,0,4,0,0,0,1,0},
              { 0,0,0,0,0,0,0,0,4,0,0,0,0,0,0,0},
              { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
              { 0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0},
              { 0,0,0,0,0,0,1,0,0,0,0,0,0,0,-1,0},
        },
        {
           { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
           { 0,0,0,0,2,0,0,0,0,0,0,0,0,2,0,0},
           { -1,0,1,0,0,0,0,4,0,1,0,0,0,0,0,0},
           { 0,0,0,0,4,0,0,0,0,4,0,0,0,0,0,0},
           { 0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0},
           { 0,0,0,0,2,0,0,0,0,0,0,2,0,0,0,0},
           { 0,0,1,0,0,0,0,0,0,0,0,0,0,4,0,0},
           { 0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,0},
           { 0,0,0,0,0,0,4,0,0,0,0,0,0,0,1,0},
           { 0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0},
           { 0,0,0,0,0,1,0,0,0,0,0,0,2,0,0,-1},
           { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        },
        {
           { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
           { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
           { 0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,0},
           { -1,0,0,3,0,4,0,0,0,0,3,0,0,0,0,0},
           { 0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0},
           { 0,0,0,0,3,0,0,0,0,0,1,0,0,2,0,0},
           { 0,0,0,0,0,0,0,0,3,0,0,0,0,0,3,0},
           { 0,0,0,3,0,4,0,0,0,0,0,0,0,0,0,0},
           { 0,0,0,0,0,0,0,0,0,0,0,2,0,0,1,0},
           { 0,0,0,1,0,0,0,2,0,0,0,0,0,0,0,0},
           { 0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0},
           { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,-1,0},
        },
        {
           { 0,0,0,0,-1,0,2,0,0,0,0,0,0,0,0,0},
           { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
           { 0,0,0,0,0,0,1,0,2,0,0,0,0,0,0,0},
           { 0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,4},
           { 0,0,0,3,0,0,0,3,0,0,3,0,0,0,0,0},
           { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
           { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0},
           { 0,0,0,4,0,0,0,0,3,0,0,0,0,0,0,0},
           { 0,0,0,0,0,0,2,1,0,0,0,4,0,0,0,0},
           { -1,0,0,0,0,3,0,0,0,0,0,0,0,3,0,0},
           { 0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0},
           { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        }
        ,
         {
           { 0,0,0,0,0,0,0,-1,0,0,0,0,0,0,0,0},
           { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
           { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
           { 0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0},
           { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
           { 0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0},
           { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
           { 0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0},
           { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
           { 0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0},
           { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
           { 0,0,0,0,0,0,0,-1,0,0,0,0,0,0,0,0},
        }
    }; //스테이지별 폭탄과 블럭의 위치를 배열로 만들음



    public GameObject block, bomb, magma; //각각 블럭 폭탄 마그마의 프리랩
    public GameObject Over; //게임 오버 UI
    public GameObject Next; //잘 있어요 전용 다음 스테이지로 가는 UI
    public PuzzleClear puzzleClear; //퍼즐 클리어
    public Sprite X;
    public Text swaping, fire, stage; //UI에 표시할 내용 스왑 가능 횟수, 점화 가능 횟수(1개로 고정), 현재 스테이지
    int snum, fnum, initialx, initialy; //스왑가능 횟수, 점화가능 횟수, 마그마의 초기위치 (x,y)
    GameObject[,] Map = new GameObject[16, 12]; //맵 내의 블럭과 폭탄들의 게임오브젝트를 저장하는 배열
    List<GameObject> Swap_List = new List<GameObject>(); //블럭이나 폭탄을 스왑할때 쓰는 리스트
    bool raymode; //마우스 입력상태를 on/off 할수 있게 만드는 불 변수
    public bool Raymode { get { return raymode; } private set { } }
    List<bombs> Bomb_List = new List<bombs>(); //폭탄이 터질때 다음 폭탄들을 저장해주는 리스트


    public TextManager textManager;

    public void Start()
    {
        GameBoard = GameObject.Find("GameBoard");
        //SoundManager.Instance.PlayBGM("Clean and Dance - An Jone");

        Swap_List.Clear();
        if (Dialogue_Proceeder.instance.CurrentEpiID == 19)
        {
            if (Dialogue_Proceeder.instance.CurrentDiaID == 8027)
            {
                Dialogue_Proceeder.instance.AddCompleteCondition(40);
                initialx = 13;
                initialy = 11;
                snum = 15;
                gameboardint = 0;
            }
            else
            {
                Dialogue_Proceeder.instance.AddCompleteCondition(42);
                initialx = 7;
                initialy = 0;
                snum = 10;
                gameboardint = 5;
            }
        }
        else if (Dialogue_Proceeder.instance.CurrentEpiID == 3)
        {
            initialx = 6;
            initialy = 0;
            snum = 9;
            gameboardint = 1;
        }
        else if (Dialogue_Proceeder.instance.CurrentEpiID == 11)
        {
            initialx = 0;
            initialy = 2;
            snum = 11;
            gameboardint = 2;
        }
        else if (Dialogue_Proceeder.instance.CurrentEpiID == 15)
        {
            initialx = 0;
            initialy = 3;
            snum = 11;
            gameboardint = 3;
        }
        else if (Dialogue_Proceeder.instance.CurrentEpiID == 16)
        {
            initialx = 4;
            initialy = 0;
            snum = 8;
            gameboardint = 4;
        }
        fnum = 1;
        swaping.text = snum.ToString();
        fire.text =  fnum.ToString();
        stage.text = "현재 퍼즐 :" + (gameboardint+1) + "번 퍼즐";
        raymode = true;
        for (int j = 0; j < 12; j++) //초기에 배열에 있는 대로 블럭을 생성하는 for문
        {
            for (int i = 0; i < 16; i++)
            {
                if (gameboardArray[gameboardint, j, i] == -1) //게임보드 배열 내에서 해당 위치 오브젝트의 유형을 판별함
                {
                    GameObject newB = GameObject.Instantiate(magma); //현재 위치에 적절한 오브젝트 생성
                    newB.transform.SetParent(GameBoard.transform);
                    newB.GetComponent<BlockBehavior>().Location = new Vector2(i, j); //자신의 상대적 위치를 해당 오브젝트 스크립트에 전달
                    Map[i, j] = newB; //오브젝트 배열에 해당 위치의 오브젝트 저장
                    newB.transform.position = new Vector3((i - 7) * 5.2f - 16f, (j - 5) * 5.2f - 3f, -1); //해당 상대 위치를 월드 좌표에 맞춰서 이동시킴
                    newB.GetComponent<BlockBehavior>().IsMagma = true; //마그마상태 on
                }
                if (gameboardArray[gameboardint, j, i] == 0)
                {
                    GameObject newB = GameObject.Instantiate(block);
                    newB.transform.SetParent(GameBoard.transform);
                    newB.GetComponent<BlockBehavior>().Location = new Vector2(i, j);
                    Map[i,j] = newB;
                    newB.transform.position = new Vector3((i - 7) *5.2f - 16f, (j - 5)*5.2f - 3f, -1);
                }
                else if (gameboardArray[gameboardint, j, i] == 1)
                {
                    GameObject newB = GameObject.Instantiate(bomb);
                    newB.transform.SetParent(GameBoard.transform);
                    newB.GetComponent<BlockBehavior>().Location = new Vector2(i, j);
                    Map[i,j] = newB;
                    newB.GetComponent<BombBehavior>().Bombtype = 0;
                    newB.transform.position = new Vector3((i - 7) * 5.2f - 16f, (j - 5) * 5.2f - 3f, -1);
                }
                else if (gameboardArray[gameboardint, j, i] == 2)
                {
                    GameObject newB = GameObject.Instantiate(bomb);
                    newB.transform.SetParent(GameBoard.transform);
                    newB.GetComponent<BlockBehavior>().Location = new Vector2(i, j);
                    Map[i, j] = newB;
                    newB.GetComponent<BombBehavior>().Bombtype = 1;
                    newB.transform.position = new Vector3((i - 7) * 5.2f - 16f, (j - 5) * 5.2f - 3f, -1);
                }
                else if (gameboardArray[gameboardint, j, i] == 3)
                {
                    GameObject newB = GameObject.Instantiate(bomb);
                    newB.transform.SetParent(GameBoard.transform);
                    newB.GetComponent<BlockBehavior>().Location = new Vector2(i, j);
                    Map[i, j] = newB;
                    newB.GetComponent<BombBehavior>().Bombtype = 2;
                    newB.transform.position = new Vector3((i - 7) * 5.2f - 16f, (j - 5) * 5.2f - 3f, -1);
                }
                else if (gameboardArray[gameboardint, j, i] == 4)
                {
                    GameObject newB = GameObject.Instantiate(bomb);
                    newB.transform.SetParent(GameBoard.transform);
                    newB.GetComponent<BlockBehavior>().Location = new Vector2(i, j);
                    Map[i, j] = newB;
                    newB.GetComponent<BombBehavior>().Bombtype = 3;
                    newB.transform.position = new Vector3((i - 7) * 5.2f - 16f, (j - 5) * 5.2f - 3f, -1);
                }
            }
        }

        GameBoard.transform.position = GameBoard.transform.position + new Vector3(13.5f, 0f, 0f);
    }
    public int getsnum() { return snum; } //스왑가능 횟수를 반환
    public void Swap(GameObject a) //스왑(자리바꿈)을 해주는 함수
    {
        BlockBehavior objBlock = a.GetComponent<BlockBehavior>();
        Debug.Log(Swap_List.Count);
        if  (snum > 0) //만약 스왑 횟수가 남았는지 판별
        {
            if (Swap_List.Count == 0) //스왑 리스트에 블럭이 없다면 그냥 블럭 하나 추가
                Swap_List.Add(a);
            else if (Mathf.Abs(objBlock.Location.x - Swap_List[0].GetComponent<BlockBehavior>().Location.x) <= 1
                && Mathf.Abs(objBlock.Location.y - Swap_List[0].GetComponent<BlockBehavior>().Location.y) <= 1) //스왑 리스트에 이미 블럭이 하나 이상 있는 경우, 현재 클릭한 블럭이 리스트의 블럭과 이웃한 것인지 판별
            {
                Swap_List.Add(a);
                Vector3 tmp = new Vector3(); //상대 좌표를 저장하기 위한 스왑용 임시변수
                Vector2 tmp1 = new Vector2(); //상대 좌표를 저장하기 위한 스왑용 임시변수
                tmp = Swap_List[0].transform.position; //여기서 부터
                tmp1 = Swap_List[0].GetComponent<BlockBehavior>().Location;
                Swap_List[0].transform.position = Swap_List[1].transform.position;
                Swap_List[0].GetComponent<BlockBehavior>().Location = Swap_List[1].GetComponent<BlockBehavior>().Location;
                Swap_List[1].transform.position = tmp;
                Swap_List[1].GetComponent<BlockBehavior>().Location = tmp1;
                Swap_List[0].GetComponent<BlockBehavior>().Select = false;
                Swap_List[1].GetComponent<BlockBehavior>().Select = false; //여기까지 서로 두 블럭의 위치정보를 스왑하는 과정, 참고로 블럭 스크립트에 갱신된 위치정보를 저장해두는 것도 잊지 말자
                GameObject ttmp = Map[(int)Swap_List[0].GetComponent<BlockBehavior>().Location.x, (int)Swap_List[0].GetComponent<BlockBehavior>().Location.y]; //스왑용 변수
                Map[(int)Swap_List[0].GetComponent<BlockBehavior>().Location.x, (int)Swap_List[0].GetComponent<BlockBehavior>().Location.y] = Map[(int)Swap_List[1].GetComponent<BlockBehavior>().Location.x, (int)Swap_List[1].GetComponent<BlockBehavior>().Location.y];
                Map[(int)Swap_List[1].GetComponent<BlockBehavior>().Location.x, (int)Swap_List[1].GetComponent<BlockBehavior>().Location.y] = ttmp; //여기까지 게임오브젝트 맵에 있는 내용을 스왑함
                if (Swap_List[0].gameObject != Swap_List[1].gameObject)
                    snum--;
                Swap_List.Clear(); //리스트 비우기
                swaping.text = snum.ToString(); //스왑 횟수 갱신
            }
            else //예외 일경우 에러나고 스왑 리스트 다 비워짐
            {
                Debug.Log("ERROR");
                Swap_List[0].GetComponent<BlockBehavior>().Select = false;
                objBlock.Select = false;
                Swap_List.Clear();
                return;
            }
        }
    }
    public void cancelSwap() //스왑리스트를 비워줘서 스왑을 캔슬시켜주는 함수( X키로 스왑캔슬함)
    {
        foreach (GameObject g in Swap_List)
        {
            Swap_List[0].GetComponent<BlockBehavior>().Select = false;
        }
        Swap_List.Clear();
    }
    public void RayMode() //마우스 입력 감지를 on/off 해주는 함수
    {
        raymode = !raymode;
    }
    public void Boom(int x, int y, GameObject b, int [,]a) //최초로 점화된 폭탄을 리스트에 넣고 폭발 페이즈로 이동 시켜주는 함수, 이후 일어나는 연쇄 폭발의 경우 EXplode와 phase코루틴이 관할한다.
    {
        if (fnum>0) //남은 점화횟수 확인
        {
            bombs newbomb = new bombs(new Vector2(x, y), b);
            Bomb_List.Add(newbomb);
            fnum--;
            fire.text = fnum.ToString();
            this.StartCoroutine(phase()); //페이스 코루틴을 호출해서 하나하나씩 터트림
        }
    }
    void result() //폭발이 끝나면 결과(클리어, 실패)에 따라서 해당 UI를 출력시켜주는 함수
    {
        if (IsComplete(initialx, initialy))
        {
            RayMode();
            StartCoroutine(Magmazation(initialx, initialy));

            if (gameboardint == 0)
            {
                Dialogue_Proceeder.instance.AddCompleteCondition(41);
                //Dialogue_Proceeder.instance.UpdateCurrentDiaIDPlus1();
                Next.SetActive(true);
                textManager.Set_Dialogue_System();
            }
            else if (gameboardint == 5)
            {
                Dialogue_Proceeder.instance.AddCompleteCondition(43);
                //Dialogue_Proceeder.instance.UpdateCurrentDiaIDPlus1();
                textManager.Set_Dialogue_System();
                puzzleClear.ClearPuzzle(SceneType.Mental, 10f);
            }
            else 
                puzzleClear.ClearPuzzle(SceneType.Mental, 5f);
        }
        else
        {
            RayMode();
            Over.SetActive(true);
        }
    }
    void Explode(int[,] a, int x, int y) //직접 폭발을 위한 여러 정보를 확인하고 폭발시켜주는 함수 만약 주변에 폭탄이 있으면 리스트에 추가시킴
    {
        for (int i = 0; i < 5; i++) //이중 for문으로 해당 폭탄의 폭발범위 배열을 읽음
            for (int j = 0; j < 5; j++)
            {
                int p = x - 2 + j;
                int q = y - 2 + i;
                if (inRange(p,q) && a[i, j] == 1) //폭발 범위 내의 블럭의 경우
                {
                    if (Map[p, q].CompareTag("Bomb")) //만약 폭탄인 경우
                    {
                        if (i == 2 && j == 2) //자기 자신인 경우 그냥 폭발
                        {
                            Map[p, q].GetComponent<BombBehavior>().IsActive = false;
                            Map[p, q].GetComponent<BombBehavior>().StartCoroutine("Blast"); 
                            gameboardArray[gameboardint, q, p] = 8;
                        }
                        else if (Map[p, q].GetComponent<BombBehavior>().IsActive) //자기 자신이 아닌, 활성화 되어있는 폭탄의 경우에는 리스트에 추가해서 수초 뒤에 폭발
                        {
                            bombs newbomb = new bombs(new Vector2(p, q), Map[p, q]);
                            Bomb_List.Add(newbomb);
                        }
                    }
                    else if (gameboardArray[gameboardint, q, p] != -1 && Map[x - 2 + j, y - 2 + i].activeSelf) //일반 블럭의 경우도 그냥 폭발
                    {
                        gameboardArray[gameboardint, q, p] = 8;
                        Map[p, q].GetComponent<BlockBehavior>().StartCoroutine("Blast");
                    }
                }
            }

    }
    public void Redrawboard(float x,float y, int[,] bombArr, bool val) //폭탄을 마우스 위에 놓았을때 폭탄의 폭발범위를 표시해주도록 하는 함수
    {
        for (int i = 0; i < 5; i++)
            for (int j = 0; j < 5; j++)
                if (bombArr[i, j] == 1 && inRange((int)x + j - 2, (int)y + i - 2) && !(i == 2 && j == 2)) //폭발 범위 내에 있는 모든 블럭 확인
                {
                    if (Map[(int)x + j - 2, (int)y + i - 2].CompareTag("Bomb") && Map[(int)x + j - 2, (int)y + i - 2].GetComponent<BombBehavior>().Inform != val) //만약 활성상태인 폭탄이 범위내에 감지되면 해당 폭탄 스크립트에서 현재 함수를 호출하도록, 그러니까 해당 폭탄도 폭발범위를 표시하도록 함수 호출
                        Map[(int)x + j - 2, (int)y + i - 2].GetComponent<BombBehavior>().showArr(val);
                    else if (!Map[(int)x + j - 2, (int)y + i - 2].GetComponent<BlockBehavior>().IsMagma) //그 외 마그마가 아닌 블록들은 붉은 판넬로 범위 표시
                    {
                        Map[(int)x + j - 2, (int)y + i - 2].GetComponent<BlockBehavior>().Inarea = val;
                        Map[(int)x + j - 2, (int)y + i - 2].GetComponent<BlockBehavior>().panel.SetActive(val);
                    }
                    
                }   
    }
    bool IsComplete(int x, int y) //게임을 클리어 했는지 클리어하지 못했는지 알아봐주는 함수 true 면 클리어
    {
        gameboardArray[gameboardint, y, x] = 9;
        if ((x - 1 >= 0 && gameboardArray[gameboardint, y, x - 1] == -1) || (x + 1 < 16 && gameboardArray[gameboardint, y, x + 1] == -1) || (y - 1 >= 0 && gameboardArray[gameboardint, y - 1, x] == -1) || (y + 1 < 12 && gameboardArray[gameboardint, y + 1, x] == -1))
        {
            return true;
        }
        if (x - 1 >= 0 && gameboardArray[gameboardint, y, x - 1] == 8)
        {
            if (IsComplete(x - 1, y))
                return true;
        }
        if (x + 1 < 16 && gameboardArray[gameboardint, y, x + 1] == 8)
        {
            if (IsComplete(x + 1, y))
                return true;
        }
        if (y - 1 >= 0 && gameboardArray[gameboardint, y - 1, x] == 8)
        {
            if (IsComplete(x, y - 1))
                return true;
        }
        if (y + 1 < 12 && gameboardArray[gameboardint, y + 1, x] == 8)
        {
            
            if (IsComplete(x, y + 1))
                return true;
        }
        return false;
    }
    public IEnumerator Magmazation(int x,int y) //마그마를 한번에 하나씩 뜸들여서 흐르게 만들도록 코루틴 사용
    {
        yield return new WaitForSeconds(0.13f); //한박자 쉬고 전후좌우에 마그마 활성화 되지 않은 상태의 블록을 찾아 마그마로 바꾼 다음, 해당 블록 주위에서 다른 활성화 되지 않은 블록 찾기
        if (inRange(x+1,y))
            if (!Map[x + 1, y].activeSelf)
                Map[x + 1, y].GetComponent<BlockBehavior>().Setmagma();
        if (inRange(x - 1, y))
            if (!Map[x - 1, y].activeSelf)
                Map[x - 1, y].GetComponent<BlockBehavior>().Setmagma();
        if (inRange(x, y - 1))
            if (!Map[x, y - 1].activeSelf)
                Map[x, y - 1].GetComponent<BlockBehavior>().Setmagma();
        if (inRange(x, y + 1))
            if (!Map[x, y + 1].activeSelf)
                Map[x, y + 1].GetComponent<BlockBehavior>().Setmagma();
        
    }
    public void GetMagmainfo(int x, int y)
    {
        StartCoroutine(Magmazation(x, y));
    }
    IEnumerator phase() //폭탄을 한번에 하나씩 뜸들여서 터트리게 만들도록 코루틴 사용
    {
        while (Bomb_List.Count > 0) //폭탄 리스트가 빌때 까지
        {
            SoundManager.Instance.PlaySE("Boom");
            Explode(Bomb_List[0].getObj().GetComponent<BombBehavior>().BombArr, (int)Bomb_List[0].getPos().x, (int)Bomb_List[0].getPos().y);//계속 Explode함수를 호출
            Bomb_List.RemoveAt(0); //폭탄하나 폭발 했으면 리스트에서 삭제
            yield return new WaitForSeconds(0.5f);//한박자 쉼
        }
        result();
        yield return null;
    }
    bool inRange (int a, int b) //폭발 범위 내에 있는지 알아봐주는 함수
    {
        if (a >= 0 && a < 16 && b >= 0 && b < 12)
            return true;
        else
            return false;
    }

    public void GotoNext()
    {
        //Dialogue_Proceeder.instance.UpdateCurrentDiaIDPlus1();
        SceneChanger.RestartScene();
    }
}