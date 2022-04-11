using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axis : MonoBehaviour
{
    [Header("참조")]
    public Control player;

    [Header("카메라 회전 설정")]
    public float distance;
    public float rotationSpeed;

    private Quaternion targetRotation;
    private Quaternion playerRotation;
    private Vector3 axisVec;
    private float gapX;
    private float gapY;

    void LateUpdate()
    {
        // 카메라 거리 유지 : Distance만큼 재우미랑 거리 유지
        axisVec = player.transform.position;
        axisVec += -transform.forward * distance;
        transform.position = axisVec;

        // 마우스 우클릭 후 좌우로 움직이면 카메라도 움직임
        // 단, 움직이는 중에는 회전 불가
        if (Input.GetMouseButton(1))
        {
            // 값을 축적
            gapY += Input.GetAxis("Mouse X") * rotationSpeed;
            gapX += Input.GetAxis("Mouse Y") * -rotationSpeed;

            // 카메라 X축 회전범위 제한
            gapX = Mathf.Clamp(gapX, -20f, 65f);

            // 회전 값을 변수에 저장
            targetRotation = Quaternion.Euler(new Vector3(gapX, gapY, 0f));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // 카메라가 움직이면 잠재우미도 회전한다.
            playerRotation = targetRotation;
            playerRotation.x = 0f;
            playerRotation.z = 0f;
            player.transform.rotation = playerRotation;
        }
    }
}
