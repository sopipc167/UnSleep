using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axis : MonoBehaviour
{
    [Header("참조")]
    public Transform playerPos;

    [Header("카메라 회전 설정")]
    public float distance;
    public float rotationSpeed;
    public float scrollScale;

    private Quaternion targetRotation;
    private Quaternion playerRotation;
    private Vector3 axisVec;
    private float gapX;
    private float gapY;

    private Camera mainCam;

    private void Awake()
    {
        mainCam = GetComponent<Camera>();
    }

    private void Update()
    {
        // 줌 인/아웃 기능
        float scroll = Input.GetAxis("Mouse ScrollWheel") * scrollScale;
        if (mainCam.fieldOfView - scroll < 40f)
        {
            mainCam.fieldOfView = 40f;
        }
        else if (mainCam.fieldOfView - scroll > 80f)
        {
            mainCam.fieldOfView = 80f;
        }
        else
        {
            mainCam.fieldOfView -= scroll;
        }

        // 마우스 우클릭 후 좌우로 움직이면 카메라도 움직임
        if (Input.GetMouseButton(1))
        {
            // 값을 축적
            gapY += Input.GetAxis("Mouse X") * rotationSpeed;
            gapX += Input.GetAxis("Mouse Y") * -rotationSpeed;

            // 카메라 X축 회전범위 제한
            gapX = Mathf.Clamp(gapX, -20f, 65f);

            // 회전 값을 변수에 저장
            targetRotation = Quaternion.Euler(new Vector3(gapX, gapY, 0f));
            playerRotation = Quaternion.Euler(new Vector3(0f, gapY, 0f));
        }

        // 잠재우미 회전
        playerPos.rotation = Quaternion.Slerp(playerPos.rotation, playerRotation, rotationSpeed * Time.deltaTime);
    }

    void LateUpdate()
    {
        // 카메라 회전
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // 카메라 거리 유지 : Distance만큼 재우미랑 거리 유지
        axisVec = playerPos.position;
        axisVec -= transform.forward * distance;
        transform.position = axisVec;
    }
}
