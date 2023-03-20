﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiaPlayer : MonoBehaviour
{
    public GameObject Dialogue_system_manager;
    public Collider2D[] dia_hit_colliders;
    public DiaInterInfo hit_info;
    public Player player;
    Vector3 MousePosition;

    public GameObject diaScene1;
    public GameObject diaScene2;
    public GameObject diaScene3;
    public GameObject diaScene4;

    public FadeInOut fade;
    public GameObject chair;
    public DiaEvent DE;

    public bool isOnce;
    public Gome gome;

    private TextManager textManager;
    private Camera mainCam;

    public MovieEffect movie;

    private void Awake()
    {
        textManager = Dialogue_system_manager.GetComponent<TextManager>();
        mainCam = Camera.main;
        isOnce = true;
    }


    void Update()
    {

        //*****************클릭*******************
        if (Input.GetMouseButtonDown(0))
        {
    
            MousePosition = Input.mousePosition;
            MousePosition = mainCam.ScreenToWorldPoint(MousePosition);

            RaycastHit2D hitted_object = Physics2D.Raycast(MousePosition, transform.forward);
            if (hitted_object)
            {
                hit_info = hitted_object.transform.GetComponent<DiaInterInfo>();


                //1. 클릭 상호작용 태그(DiaInterClick)이고 2. 대화 UI가 꺼져있고 3.상호작용 반경 내에 있으면 클릭 상호작용 대사 출력
                if (hitted_object.transform.CompareTag("DiaInterClick")
                    && textManager.DiaUI.activeSelf == false
                    && Vector3.Distance(transform.position, hitted_object.transform.position) <= hit_info.Interaction_distance
                    && Vector3.Distance(transform.position, MousePosition) <= 11.5f) {
                    DialogueInteraction(hit_info);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        //***************충돌*********************
        dia_hit_colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), 3.0f);
        if (dia_hit_colliders.Length > 0)
        {            
            for (int i = 0; i < dia_hit_colliders.Length; i++)
            {
                if (dia_hit_colliders[i].CompareTag("DiaInterCollision")
                    && textManager.DiaUI.activeSelf == false)
                {
                    hit_info = dia_hit_colliders[i].transform.GetComponent<DiaInterInfo>();
                    DialogueInteraction(hit_info);
                    if (!hit_info.OnlyOnce[0] && isOnce)
                    {
                        isOnce = false;
                    }
                }
                else if (dia_hit_colliders[i].CompareTag("SceneOver"))
                {
                    diaScene1.SetActive(false);
                    diaScene2.SetActive(true);
                    DE.Outline_false();
                    DE.next_flase = 701;
                    DE.next_true = 700;
                    DE.ob[7].SetActive(true);
                    //player.transform.localPosition = new Vector3(-5.05f, -1.38f, 0);
                    chair.transform.localPosition = new Vector3(5.87f, -2.76f, 0);
                }
                else if(dia_hit_colliders[i].tag == "SceneOver_2")
                {
                    DE.next_flase = 707;
                    DE.next_true = 706;
                    diaScene2.SetActive(false);
                    diaScene3.SetActive(true);
                }
                else if(dia_hit_colliders[i].tag == "Gome")
                {
                    gome.touchPlayer();
                }
                else if(dia_hit_colliders[i].tag == "Chair")
                {
                    DE.Move(2, new Vector3(7.74f, -1.86f, 0), new Vector3(0, 0, -90));
                    DE.chair.enabled = false;
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
            SceneChanger.ChangeScene(hit.sceneType);
        }

        //Debug.Log("상호작용 대화 실행");

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

            if (isOnce && Dialogue_Proceeder.instance.AlreadyDone(hit_Diaid[i])) //한번만 실행되는 대화, 이미 실행되었으면 넘긴다.
            {
                Debug.Log("hit_Diaid_inOnce_true: " + hit_Diaid[i]);
                continue;
            }
            else
            {
                if (hit_info.isAuto && !textManager.isMovieIn)
                {
                    if(DE.outline != 0)
                        DE.Outline_false();
                    movie.MovieFrameIn();
                    textManager.isMovieIn = true;
                    if (!hit_info.isMany)
                        textManager.isMovieOut = true;
                }
                Debug.Log("hit_Diaid_inOnce_false: " + hit_Diaid[i] + " isOnce: " + isOnce +
                    " complete: " + Dialogue_Proceeder.instance.AlreadyDone(hit_Diaid[i]));
            }


            Debug.Log("고미 stop");
            player.col.enabled = false;
            player.isStop = true;
            gome.isStart = false;
            SpriteOutline.instance.isStop = true;

            //실행 조건 가져옴
            int[] conditions = textManager.ReturnDiaConditions(hit_Diaid[i]);

            //조건에 만족하면
            if (Dialogue_Proceeder.instance.Satisfy_Condition(conditions))
            {

                
                if (textManager.EffectEnd)
                {
                    Dialogue_Proceeder.instance.UpdateCurrentDiaID(hit_Diaid[i]); //현재 대화묶음id로 설정 후 함수 종료
                    textManager.SetDiaInMap();
                    textManager.Increasediaindex = true; //대사 인덱스 넘어갈 수 있게 함.
                }
                    
              
                isOnce = true;
                Debug.Log("대화묶음: " + hit_Diaid[i] + " isOnce: " + isOnce);

                return;
            }

        }
    }
}
