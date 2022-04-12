using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    [Header("참조")]
    public GameObject Dia_UI; 
    public GameObject Select_UI;

    [Header("잠재우미 속도 설정")]
    public float speed;
    public float turnSpeed;

    private Camera cam;
    private Ray ray;
    private bool flag;

    private Quaternion targetRot;
    private Vector3 destination;
    private Vector3 offset;

    private Animator animator;

    void Start()
    {
        cam = Camera.main;
        animator = GetComponent<Animator>();
        targetRot = transform.rotation;
    }

    private void FixedUpdate()
    {
        if (!flag) return;
        flag = false;

        // 클릭해서 부딪히면 좌표 저장
        if (Physics.Raycast(ray, out RaycastHit hit, 10000f))
        {
            destination.x = hit.point.x;
            destination.y = hit.point.y + 2f;
            destination.z = hit.point.z;
        }
    }

    void Update()
    {
        if (Dia_UI.activeSelf || Select_UI.activeSelf) return;

        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 클릭 위치 좌표 받아옴
            ray = cam.ScreenPointToRay(Input.mousePosition);
            flag = true;

            // 화면 기준 좌, 우 클릭에 따라 잠재우미 좌우반전
            if (cam.ScreenToViewportPoint(Input.mousePosition).x < 0.5f)
            {
                transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
            }
            else
            {
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
        }

        // 어느정도 차이가 있으면 targetPos로 이동
        offset = destination - transform.position;
        if (offset.sqrMagnitude >= 0.5f)
        {
            animator.SetBool("Running", true);
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            targetRot = Quaternion.LookRotation(offset);
            targetRot = Quaternion.Euler(targetRot.eulerAngles.x, targetRot.eulerAngles.y, 0f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("Running", false);
        }
    }
}
