﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    [Header("참조")]
    public GameObject Dia_UI; 
    public GameObject Select_UI;
    public GameObject cinematic1;
    public GameObject cinematic2;
    public GameObject Log_UI;
    public GameObject goToPuzzle;
    public ParticleSystem clickParticle;
    public Transform clickPoolPos;

    [Header("밟기 가능 레이어마스크")]
    public LayerMask canMoveMask;

    [Header("1주기 움직이는데 걸리는 시간")]
    public float waveCyclePerSecond;

    private DiaInterActor actor;
    private PlayerMovement movement;
    private IEnumerator coroutine = null;
    private float sinValue = 0f;

    private Camera cam;
    private Ray ray;

    private Animator animator;

    private readonly ParticleSystem[] pool = new ParticleSystem[20];
    private int poolIdx = 0;

    void Start()
    {
        cam = Camera.main;
        animator = GetComponent<Animator>();
        actor = GetComponent<DiaInterActor>();
        movement = transform.parent.GetComponent<PlayerMovement>();
        for (int i = 0; i < 20; ++i)
        {
            pool[i] = Instantiate(clickParticle.gameObject, clickPoolPos).GetComponent<ParticleSystem>();
        }
    }

    void Update()
    {
        if (Dia_UI.activeSelf || Select_UI.activeSelf || goToPuzzle.activeSelf ||
            cinematic1.activeSelf || cinematic2.activeSelf || Log_UI.activeSelf) return;

        // 마우스 좌클릭 시
        if (Input.GetMouseButtonDown(0))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 500f, canMoveMask))
            {
                pool[poolIdx].gameObject.transform.position = new Vector3(hit.point.x, hit.point.y + 1f, hit.point.z);
                if (pool[poolIdx].isPlaying) pool[poolIdx].Clear();
                pool[poolIdx++].Play();
                poolIdx = poolIdx > 19 ? 0 : poolIdx;

                // 화면 기준 좌, 우 클릭에 따라 잠재우미 좌우반전
                if (cam.ScreenToViewportPoint(Input.mousePosition).x < 0.5f)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                else
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }

                // 이동 시작
                animator.SetBool("Running", true);
                if (coroutine != null) StopCoroutine(coroutine);
                coroutine = MovePlayerCoroutine(hit.point);
                StartCoroutine(coroutine);
            }
        }
    }

    private IEnumerator MovePlayerCoroutine(Vector3 clickPos)
    {
        bool condition;
        do
        {
            sinValue += Mathf.PI * (2f / waveCyclePerSecond) * Time.deltaTime;
            transform.localPosition = new Vector3(0f, Mathf.Sin(sinValue), 0f);
            condition = movement.MovePlayer(clickPos);
            yield return null;
        } while (condition && !actor.isInteracting);

        actor.isInteracting = false;
        animator.SetBool("Running", false);
        coroutine = null;
    }
}
