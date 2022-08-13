using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaInterInfo : MonoBehaviour
{
    //이 오브젝트를 클릭/충돌/이벤트 발생 시 활성화할 대화 묶음 id.
    //인스펙터에다 하드코딩 해주세요

    //한 오브젝트에서 여러가지 이벤트가 발생할 경우
    //흐름 진행 순서대로 배열에 넣어주세요. 

    public int[] Obj_Diaid;
    public bool[] OnlyOnce;
    public float Interaction_distance;

    public bool isChangeScene; //이 오브젝트와 상호작용으로 씬 전환을 하는 경우
    public string ChangeSceneName; //전환되는 씬 이름을 적어주세요

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawSphere(transform.position, Interaction_distance);
    //    //Scene 화면의 Gizmos 누르면 상호작용 범위가 구로 표시됩니다. 
    //}

}
