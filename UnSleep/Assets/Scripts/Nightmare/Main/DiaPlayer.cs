using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiaPlayer : MonoBehaviour
{
    public GameObject Dialogue_system_manager;
    public Collider2D[] dia_hit_colliders;
    private DiaInterInfo hit_info;
    public Player player;
    Vector3 MousePosition;

    public GameObject diaScene1;

    // Update is called once per frame
    void Update()
    {
        if (!Dialogue_system_manager.GetComponent<TextManager>().Increasediaindex)
        {
            player.isStop = false;
        }

        //*****************클릭*******************
        if (Input.GetMouseButtonDown(0))
        {
            MousePosition = Input.mousePosition;
            MousePosition = Camera.main.ScreenToWorldPoint(MousePosition);

            RaycastHit2D hitted_object = Physics2D.Raycast(MousePosition, transform.forward);
            if (hitted_object)
            {
                hit_info = hitted_object.transform.GetComponent<DiaInterInfo>();

                /*if(hit_info.OnlyOnce[0] && hit_info.Obj_Diaid[0] == 704)
                {
                    lightClick2.SetActive(true);
                    lightClick1.SetActive(false);
                }*/

                //1. 클릭 상호작용 태그(DiaInterClick)이고 2. 대화 UI가 꺼져있고 3.상호작용 반경 내에 있으면 클릭 상호작용 대사 출력
                if (hitted_object.transform.tag.Equals("DiaInterClick")
                    && Dialogue_system_manager.GetComponent<TextManager>().DiaUI.activeSelf == false
                    && Vector3.Distance(transform.position, hitted_object.transform.position) <= hit_info.Interaction_distance)
                    DialogueInteraction(hit_info);
            }
        }


        //***************충돌*********************
        dia_hit_colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), 3.0f);
        if (dia_hit_colliders.Length > 0)
        {
            for (int i = 0; i < dia_hit_colliders.Length; i++)
            {
                if (dia_hit_colliders[i].tag == "DiaInterCollision" && Dialogue_system_manager.GetComponent<TextManager>().DiaUI.activeSelf == false)
                {
                    hit_info = dia_hit_colliders[i].transform.GetComponent<DiaInterInfo>();
                    DialogueInteraction(hit_info);
                }else if(dia_hit_colliders[i].tag == "SceneOver")
                {
                    diaScene1.SetActive(false);
                }
            }
        }
    }

    public void DialogueInteraction(DiaInterInfo hit)
    {
        if (hit == null)
            return;

        if (hit.isChangeScene) //상호작용으로 씬 전환이 이루어지는 경우
        {
            Debug.Log("씬 전환");
            SceneManager.LoadScene(hit.ChangeSceneName);
        }

        Debug.Log("상호작용 대화 실행");

        int[] hit_Diaid = hit.Obj_Diaid;
        int event_cnt = hit_Diaid.Length;


        //뒷쪽 이벤트가 흐름 상 조건의 개수가 많거나, 뒷 순번의 조건을 가지고 있기 때문에
        //뒤부터 검증
        //ex) 이벤트 A, B, C 3가지가 한 오브젝트에서 일어나는 상황
        // A = id : 1901, 조건 : 1900
        // B = id : 1905, 조건 : 1903, 1904
        // C = id : 1906, 조건 : 1903, 1904, 1905

        // 완료 = {1900, 1901, 1902, 1903, 1904}인 상황일 때 -> C 불충족 B 충족 -> B 실행
        // 완료 = {1900, 1901, 1902, 1903, 1904, 1905}인 상황일 때 -> C 충족 -> C 실행

        for (int i = event_cnt - 1; i >= 0; i--)
        {
            if (hit.OnlyOnce[i] && Dialogue_Proceeder.instance.AlreadyDone(hit_Diaid[i])) //한번만 실행되는 대화, 이미 실행되었으면 넘긴다.
                continue;

            player.col.enabled = false;
            player.isStop = true;

            //실행 조건 가져옴
            int[] conditions = Dialogue_system_manager.GetComponent<TextManager>().ReturnDiaConditions(hit_Diaid[i]);

            //조건에 만족하면
            if (Dialogue_Proceeder.instance.Satisfy_Condition(conditions))
            {
                Dialogue_Proceeder.instance.UpdateCurrentDiaID(hit_Diaid[i]); //현재 대화묶음id로 설정 후 함수 종료
                Dialogue_system_manager.GetComponent<TextManager>().SetDiaInMap();
                Dialogue_system_manager.GetComponent<TextManager>().Increasediaindex = true; //대사 인덱스 넘어갈 수 있게 함.

                return;
            }

        }


        Debug.Log("실행 조건 불충분"); //디버깅용 
    }

}


