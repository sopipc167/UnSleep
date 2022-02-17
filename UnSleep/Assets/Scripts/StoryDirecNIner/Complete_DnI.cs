using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Complete_DnI : MonoBehaviour
{
    public int DnI_id; //해당 연출/상호작용의 id. 프리팹 만들시 인스펙터로 입력
    TextManager textManager;

    private void Start()
    {
        textManager = GameObject.Find("Manager").GetComponent<TextManager>();
    }

    //어찌저찌 쿵짝쿵짝해서 이 연출이 끝날시에 이 함수를 호출하여 대화로 돌아온다.
    //본 연출의 id를 Dialogue_Proceeder의 완료 리스트에 추가한다.
    public void Complete_Direc_and_Inter()
    {
        Dialogue_Proceeder.instance.AddCompleteCondition(DnI_id); //완료 리스트에 추가

        //스토리로 복귀
        textManager.dialogues_index++; //대사 인덱스 하나 증가
        textManager.isDnI = false; 
        textManager.Increasediaindex = true; //대사 인덱스 증가 잠금 해제
        textManager.Set_Dialogue_System(); //대화 시스템 갱신 
       // -> 자연스럽게 스토리로 돌아오기 위해 필요

        Destroy(gameObject); //끝나면 삭제
    }
}
