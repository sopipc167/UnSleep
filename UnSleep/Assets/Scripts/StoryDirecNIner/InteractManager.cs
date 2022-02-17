using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StoryInteract : MonoBehaviour
{
    public int DnI_id;   //해당 연출 및 상호작용의 id

    public abstract bool IsCompelete();
}

public class InteractManager : MonoBehaviour
{
    private StoryInteract curInteraction;
    private TextManager textManager;

    private void Start()
    {
        textManager = GetComponent<TextManager>();
    }

    public void StartInteraction(StoryInteract complete)
    {
        curInteraction = complete;
        StartCoroutine(CompleteCoroutine());
    }

    private IEnumerator CompleteCoroutine()
    {
        yield return new WaitUntil(() => curInteraction.IsCompelete());
        Complete_Direc_and_Inter();
    }

    //어찌저찌 쿵짝쿵짝해서 이 연출이 끝날시에 이 함수를 호출하여 대화로 돌아온다.
    //본 연출의 id를 Dialogue_Proceeder의 완료 리스트에 추가한다.
    private void Complete_Direc_and_Inter()
    {
        Dialogue_Proceeder.instance.AddCompleteCondition(curInteraction.DnI_id); //완료 리스트에 추가

        //스토리로 복귀
        //textManager.dialogues_index++; //대사 인덱스 하나 증가
        //textManager.isDnI = false;
        //textManager.Increasediaindex = true; //대사 인덱스 증가 잠금 해제
        //textManager.Set_Dialogue_System(); //대화 시스템 갱신 
        // -> 자연스럽게 스토리로 돌아오기 위해 필요

        textManager.CombackfromDnI(); //저쪽으로 옮겼습니다. 이유는 대화묶음 속 상호작용 위치에 따라 작동을 다르게 해서입니당

        //>> 이거 왜 삭제해요???  
        //-----------쓸모 없어서요? 아... 굳이 삭제할 필요 없구나
        curInteraction.gameObject.SetActive(false);
    }
}