using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiaInterActor : MonoBehaviour
{
    public GameObject Dialogue_system_manager;
    public Collider[] dia_hit_colliders;
    private DiaInterInfo hit_info;

    private TextManager textManager;
    private Camera mainCam;

    private void Awake()
    {
        textManager = Dialogue_system_manager.GetComponent<TextManager>();
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //*****************클릭*******************
        if (Input.GetMouseButtonDown(0))
        {
            Ray dia_ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(dia_ray, out RaycastHit hitted_object))
            {
                hit_info = hitted_object.transform.GetComponent<DiaInterInfo>();
                //1. 클릭 상호작용 태그(DiaInterClick)이고 2. 대화 UI가 꺼져있고 3.상호작용 반경 내에 있으면 클릭 상호작용 대사 출력 + 220405추가 선택지 UI도 꺼져있어야 함
                if (hitted_object.transform.CompareTag("DiaInterClick")
                    && textManager.DiaUI.activeSelf == false
                    && textManager.SelectUI.activeSelf == false
                    && Vector3.Distance(transform.position, hitted_object.transform.position) <= hit_info.Interaction_distance)
                    DialogueInteraction(hit_info);
            }
        }
    }

    private void FixedUpdate()
    {
        //***************충돌*********************
        dia_hit_colliders = Physics.OverlapSphere(transform.position, 3.0f);
        if (dia_hit_colliders.Length > 0)
        {
            for (int i = 0; i < dia_hit_colliders.Length; i++)
            {
                if (dia_hit_colliders[i].CompareTag("DiaInterCollision") && textManager.DiaUI.activeSelf == false)
                {
                    hit_info = dia_hit_colliders[i].transform.GetComponent<DiaInterInfo>();
                    DialogueInteraction(hit_info);
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
            //Debug.Log("씬 전환");
            SceneManager.LoadScene(hit.ChangeSceneName);
        }

       // Debug.Log(hit.gameObject.name);



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

            //실행 조건 가져옴
            int[] conditions = textManager.ReturnDiaConditions(hit_Diaid[i]);

            //조건에 만족하면
            if (Dialogue_Proceeder.instance.Satisfy_Condition(conditions))
            {
                Debug.Log("상호작용 대화 실행");
                Dialogue_Proceeder.instance.UpdateCurrentDiaID(hit_Diaid[i]); //현재 대화묶음id로 설정 후 함수 종료
                textManager.SetDiaInMap();
                textManager.Increasediaindex = true; //대사 인덱스 넘어갈 수 있게 함.

                return;
            }

        }


        //Debug.Log("실행 조건 불충분"); //디버깅용 
    }

}


