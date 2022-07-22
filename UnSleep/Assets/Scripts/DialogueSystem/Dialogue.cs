using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Dialogue 클래스 스크립트
[System.Serializable]
public class Dialogue //대사 단위
{
    public string name;             //말하는 이
    public string contexts;        //대사
    public int portrait_emotion; //초상화 표정 
    public int portrait_position; //발화자 위치

    public bool isSelect;     //선택지 여부
    public int nextDiaKey; //선택지 선택 시 다음 대화 묶음

    public int layoutchange; //레이아웃 변화
    public string BG; //배경
    public string Content; //일러스트명, 상호작용 명

    public string SE; //효과음

}


//Dialogue 를 배열로 관리하는 클래스
[System.Serializable]
public class DialogueEvent //대화묶음 단위
{
    public int SceneNum; //실행 씬 넘버 (1 - DialogueTest 2 - MentalWorld 3 - ClockTower 4 - Volcano 5 - Lake 6 - Cliff 7 - Cave
    public string Place; //장소
    public int DiaKey; //대화 묶음
    public int[] Condition; //대화 발생 조건

    //public bool isStory; //스토리일 경우 true, 맵일 경우 false

    public string BGM;


    public Dialogue[] dialogues;
    public int dialogues_size;
}


//custom class는 inspector상에 보이지 않으므로 [System.Serializable]을 써서 
//inspector에서 수정할 수 있도록 해준다.
