using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dialogue_Proceeder : MonoBehaviour
{
    public static Dialogue_Proceeder instance;

    public int CurrentEpiID; // 현재 에피소드 id (0~19)
    public int CurrentDiaID; // 현재 대화 id
    public int CurrentDiaIndex = 0; // 현재 대화 id index

    public List<int> Complete_Condition = new List<int>(); //완료 조건 리스트
    public bool End; //일기장 펄럭펄럭용. 에피소드 끝 -> 일기장 전환시 "E"로 설정.

    private DialogueParser dialogueParser; //음... 참조가 필요하겠군,,
    public SceneType CurrentPuzzle; // 이제 가야하는 퍼즐 명
    private SceneType[] PuzzleList; // 이 에피소드에서 가야하는 퍼즐
    private int puzzleIdx;  // 퍼즐리스트 현재 가리키고 있는 인덱스
  
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;

            
        }
         DontDestroyOnLoad(gameObject);
    }

    public void UpdateCurrentEpiID(int updateid) //현재 에피소드 id 갱신
    {
        CurrentEpiID = updateid;
    }

    public void UpdateCurrentDiaID(int updateid) //현재 대화 id 갱신
    {
        CurrentDiaID = updateid;
    }

    public void UpdateCurrentDiaIDPlus1() //현재 대화 id 갱신
    {
        CurrentDiaID++;
        CurrentDiaIndex = 0;
    }

    public void AddCompleteCondition(int complete) //대화 종료시 완료 조건 리스트에 대화 id 추가
    {
        if (!Complete_Condition.Contains(complete))
            Complete_Condition.Add(complete);

        etcCase(complete);
    }

    public void RemoveCompleteCondition(int complete) //로그 되돌아가기, 동굴 중간포인트부터 실행시 완료 조건 도르마무
    {
        if (Complete_Condition.Contains(complete))
            Complete_Condition.Remove(complete);
    }

    public void RemoveAllCompleteCondition()
    {
        Complete_Condition.Clear();
    }


    public bool Satisfy_Condition(int[] condition) //input: 조건 배열
    {
        if (condition[0] == 0) //조건 란이 공란 -> 크기 1짜리 int 배열. 내용은 0. 
            return true;

        for (int i=0; i < condition.Length; i++)
        {
            if (condition[i] < 0) //-가 붙었을 시 
            {
                if (Complete_Condition.Contains(-1 * condition[i])) //완료 조건에 존재하면 false 반환
                    return false;
            } 
            else
            {
                if (!Complete_Condition.Contains(condition[i]))
                    return false; //하나라도 만족 못할시 false 반환

            }
        }

        return true; //모든 조건 만족 시 true 반환
    }

    public bool AlreadyDone(int id) //DiaInterInfo OnlyOne = true인 경우 사용. 딱 한번만 실행할 대화일 경우, 이미 완수했으면 true.
    {
        if (Complete_Condition.Contains(id))
            return true; //이미 완수했으면 true 반환
        else
            return false; 
    }

    public void InitEpi()
    {
        StartCoroutine(InitEpiCoroutine());
    }

    private IEnumerator InitEpiCoroutine()
    {
        // 씬 이름 결정
        string sceneName;
        if (CurrentEpiID == 1 || CurrentEpiID == 6)
            sceneName = "Nightmare";
        else
            sceneName = "DialogueTest";

        // 비동기 씬 로딩
        var scene = SceneManager.LoadSceneAsync(sceneName);
        WaitForSeconds delay = new WaitForSeconds(0.1f);

        // 씬로딩이 끝날 때까지 대기
        scene.allowSceneActivation = false;
        do
        {
            yield return delay;
        } while (scene.progress < 0.9f);

        // 씬을 켜고 모든 게임 오브젝트가 배치될 떄까지 대기
        scene.allowSceneActivation = true;
        do
        {
            yield return delay;
        } while (scene.progress < 0.99f);

        // dialogueParser를 찾고 퍼즐 순서를 미리 결정

        dialogueParser = GameObject.FindGameObjectWithTag("GameController").GetComponent<DialogueParser>();
        PuzzleList = dialogueParser.getPuzzle(); // 파서에서 퍼즐 이름 배열을 받아옴
        foreach (var item in PuzzleList)
        {
            Debug.Log(item);
        }
        puzzleIdx = -1;
        CurrentPuzzle = PuzzleList[++puzzleIdx]; // 첫번째 것으로 세팅


        // 굳이 제외할 필요 있을까 싶어서 빼버림
        //
        // if (CurrentEpiID != 6 && CurrentEpiID != 10) //퍼즐이 없는 공포에피 제외
        // {
        // 
        // }
    }

    public void ClearPuzzle()
    {
        ++puzzleIdx;
        if (PuzzleList.Length != puzzleIdx)
        {
            CurrentPuzzle = PuzzleList[puzzleIdx];
        }
        else
        {
            CurrentPuzzle = SceneType.None;
        }
    }

    public void SetCurrentDiaID() //<------------ 일기장 -> 스토리 진입 시 호출------------->
    {
        switch (CurrentEpiID)
        {
            case 0: CurrentDiaID = 601; break;
            case 1: CurrentDiaID = 701; break;
            case 2: CurrentDiaID = 901; break;
            case 3: CurrentDiaID = 1501; break;
            case 4: CurrentDiaID = 1801; break;

            case 5: CurrentDiaID = 1901; break;
            case 6: CurrentDiaID = 2001; break;
            case 7: CurrentDiaID = 2001; break;
            case 8: CurrentDiaID = 2301; break;
            case 9: CurrentDiaID = 2401; break;

            case 10: CurrentDiaID = 2701; break;
            case 11: CurrentDiaID = 2701; break;
            case 12: CurrentDiaID = 3101; break;
            case 13: CurrentDiaID = 3201; break;
            case 14: CurrentDiaID = 4501; break;

            case 15: CurrentDiaID = 5001; break;
            case 16: CurrentDiaID = 5601; break;
            case 17: CurrentDiaID = 6501; break;
            case 18: CurrentDiaID = 7001; break;
            case 19: CurrentDiaID = 8001; break;
        }
    }



    public void etcCase(int id) //진짜진짜 예외 처리
    {

        if (CurrentEpiID == 7 &&(id == 2009 || id == 2010))
            Complete_Condition.Add(7);
    }
}
