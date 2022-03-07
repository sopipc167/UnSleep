using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    private Behaviour behaviour; //behaviour 스크립트 **본 프로젝트 만들면 이름 변경하기 (디폴트값이랑 겹쳐서 경고 뜸)
    private Camera mainCamera; //카메라
    public Vector3 targetPos; //이동할 좌표

    GameObject Dia_UI; //대화UI
    GameObject Select_UI;


    void Start()
    {
        behaviour = GetComponent<Behaviour>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Dia_UI = GameObject.Find("DiaUI");
        Select_UI = GameObject.Find("SelectUI");
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0)) //마우스 입력
        {
        
                //마우스 클릭 위치 좌표 받아옴
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                Vector3 Screenhit = Camera.main.ScreenToViewportPoint(Input.mousePosition);

                RaycastHit hit;

                if (Screenhit.x < 0.5)
                    transform.localScale = new Vector3(-1, 1, 1);
                else
                    transform.localScale = new Vector3(1, 1, 1);

                if (Physics.Raycast(ray, out hit, 10000f)) //클릭해서 부딪히면
                {
                    targetPos.x = hit.point.x; //거기 좌표 저장해서
                    targetPos.y = hit.point.y + 2;
                    targetPos.z = hit.point.z;

                }

            


        }



        
        if (Dia_UI.activeSelf == false)
        {

        if (behaviour.Run(targetPos)) //Run에 전달하고, 리턴 bool값이 true면 
            {
                behaviour.Turn(targetPos); //Turn까지 실행
            }
        }

         





    }

}
