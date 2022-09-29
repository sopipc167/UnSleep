using System.Collections;
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

    [Header("잠재우미 속도 설정")]
    public float speed;
    public float turnSpeed;

    [Header("밟기 가능 레이어마스크")]
    public LayerMask canMoveMask;

    private DiaInterActor actor;
    private Camera cam;
    private Ray ray;

    private Vector3 destination;
    private float sinValue = 0f;
    private float curY;

    private Animator animator;
    private IEnumerator coroutine = null;


    void Start()
    {
        cam = Camera.main;
        animator = GetComponent<Animator>();
        actor = GetComponent<DiaInterActor>();
    }

    void Update()
    {
        if (Dia_UI.activeSelf || Select_UI.activeSelf ||
            cinematic1.activeSelf || cinematic2.activeSelf || Log_UI.activeSelf) return;

        // 마우스 좌클릭 시
        if (Input.GetMouseButtonDown(0))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 500f, canMoveMask))
            {
                destination = hit.point;
                curY = destination.y + 3f;

                // 화면 기준 좌, 우 클릭에 따라 잠재우미 좌우반전
                if (cam.ScreenToViewportPoint(Input.mousePosition).x < 0.5f)
                {
                    transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
                }
                else
                {
                    transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }

                // 이동 시작
                animator.SetBool("Running", true);
                if (coroutine != null) StopCoroutine(coroutine);
                coroutine = MovePlayerCoroutine();
                StartCoroutine(coroutine);
            }
        }
    }

    private IEnumerator MovePlayerCoroutine()
    {
        // 1초에 2 ~ 4 움직이게
        do
        {
            sinValue += Mathf.PI * 2 * Time.deltaTime;
            destination.y = curY + Mathf.Sin(sinValue);
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            yield return null;
        } while (Vector3.Distance(transform.position, destination) > 0.1f && !actor.isInteracting);

        actor.isInteracting = false;
        animator.SetBool("Running", false);
        coroutine = null;
    }
}
