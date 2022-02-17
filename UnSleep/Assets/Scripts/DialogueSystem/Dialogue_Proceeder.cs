using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Proceeder : MonoBehaviour
{
    public static Dialogue_Proceeder instance;

    public int CurrentEpiID;
    public int CurrentDiaID = 1801; //현재 대화 id
    public List<int> Complete_Condition = new List<int>(); //완료 조건 리스트
    public string End; //일기장 펄럭펄럭용. 에피소드 끝 -> 일기장 전환시 "E"로 설정.
  
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

    public void UpdateCurrentDiaID(int updateid) //현재 대화 id 갱신
    {
        CurrentDiaID = updateid;
    }

    public void AddCompleteCondition(int complete) //대화 종료시 완료 조건 리스트에 대화 id 추가
    {
        if (!Complete_Condition.Contains(complete))
            Complete_Condition.Add(complete);
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



}
