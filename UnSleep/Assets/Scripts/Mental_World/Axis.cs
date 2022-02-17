using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axis : MonoBehaviour
{
    public Quaternion TargetRotation;
    public Transform CameraVector;
    public Transform player;
    private Behaviour behaviour;
    private Control control;

    public float RotationSpeed;
    public float ZoomSpeed;
    public float Distance;

    private Vector3 AxisVec;
    private Vector3 Gap;

    private Transform MainCamera;

    void Start()
    {
        MainCamera = Camera.main.transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        behaviour = player.GetComponent<Behaviour>();
        control = player.GetComponent<Control>();

    }

    // Update is called once per frame
    void Update()
    {
        DisCamera();
        CameraRoatation();

    }

    void DisCamera() //카메라 거리 유지 : Distance만큼 재우미랑 거리 유지
    {
        AxisVec = transform.forward * -1; // * -1 은 왜하는거지? 
        AxisVec *= Distance;
        MainCamera.position = transform.position + AxisVec;
    }

    void CameraRoatation()
    {
        if (transform.rotation != TargetRotation)
            transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, RotationSpeed * Time.deltaTime);

        if (Input.GetMouseButton(1))
        {
            // 값을 축적.
            Gap.x += Input.GetAxis("Mouse Y") * RotationSpeed * -1;
            Gap.y += Input.GetAxis("Mouse X") * RotationSpeed;

            // 카메라 회전범위 제한.
            Gap.x = Mathf.Clamp(Gap.x, -5f, 85f);
            // 회전 값을 변수에 저장.
            TargetRotation = Quaternion.Euler(Gap);

            // 카메라벡터 객체에 Axis객체의 x,z회전 값을 제외한 y값만을 넘긴다.
            Quaternion q = TargetRotation;
            q.x = q.z = 0;
            CameraVector.transform.rotation = q;

            if (!behaviour.Run(control.targetPos))
            {
                player.transform.rotation = q;

            }

        }
    }
}
