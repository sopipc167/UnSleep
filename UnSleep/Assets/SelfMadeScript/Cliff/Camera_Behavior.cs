using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Behavior : MonoBehaviour //카메라의 움직임을 관장하는 스크립트
{
    Camera cam;
    float Scroll; //마우스 휠의 움직임을 저장하는 변수
    Vector3 mousePos; //마우스 위치
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        Scroll = 0;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        Zooming();
        Moving();
    }
    void Moving() //마우스가 화면 밖으로 벗어나려 하면 해당 방향으로 카메라를 움직임
    {
        Vector3 movingPos= cam.ScreenToViewportPoint(mousePos);
        if (movingPos.x < 0.1 && cam.transform.position.x > -5)
        {
            cam.transform.Translate(Vector3.left * Time.deltaTime * 5);
        }
        else if (movingPos.x > 0.9 && cam.transform.position.x < 5)
        {
            cam.transform.Translate(Vector3.right * Time.deltaTime * 5);
        }
        if (movingPos.y < 0.1 && cam.transform.position.y > -3)
        {
            cam.transform.Translate(Vector3.down * Time.deltaTime * 5);
        }
        else if (movingPos.y > 0.9 && cam.transform.position.y < 3)
        {
            cam.transform.Translate(Vector3.up * Time.deltaTime * 5);
        }
    }
    void Zooming() //마우스휠을 움직여서 줌인 줌아웃을 하게 해주는 스크립트 마우스를 중심으로 하도록 줌인을 할때는 카메라가 살짝 움직인다.
    {
        Vector3 FocusPos = cam.ScreenToWorldPoint(mousePos);
        Vector3 FocusVec = FocusPos - cam.transform.position;
        if (cam.orthographicSize >= 2 && cam.orthographicSize <= 10)
        {
            Scroll = Input.GetAxis("Mouse ScrollWheel") * 5;
            if (!(Scroll < 0 && cam.orthographicSize == 2) && !(Scroll > 0 && cam.orthographicSize == 10))
            {
                if (Scroll < 0)
                    cam.transform.Translate(FocusVec * Time.deltaTime * 5);
                cam.orthographicSize += Scroll;
            }    
        }
        else if (cam.orthographicSize < 2)
        {
            cam.orthographicSize = 2;
        }
        else
        {
            cam.orthographicSize = 10;
        }
    }
}
