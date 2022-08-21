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

    [Header("잠재우미 속도 설정")]
    public float speed;
    public float turnSpeed;

    [Header("밟기 가능 레이어마스크")]
    public LayerMask canMoveMask;

    private DiaInterActor actor;
    private Camera cam;
    private Ray ray;

    private Vector3 destination;

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
            cinematic1.activeSelf || cinematic2.activeSelf) return;

        // 마우스 좌클릭 시
        if (Input.GetMouseButtonDown(0))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 500f, canMoveMask))
            {
                // 좌표 저장, 실제 충돌보다 5만큼 더 위에 있게
                destination = hit.point;
                destination.y += 5f;

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
        do
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            yield return null;
        } while (Vector3.Distance(transform.position, destination) > 0.1f && !actor.isInteracting);

        actor.isInteracting = false;
        animator.SetBool("Running", false);
        coroutine = null;
    }
}
