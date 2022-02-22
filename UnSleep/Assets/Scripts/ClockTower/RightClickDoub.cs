using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class RightClickDoub : MonoBehaviour
{
   // private RectTransform rectTransform;

    //public float curRightDown = 0f;
    //public float maxRightDown = 1f;

    //public GameObject RightDowngage;
    //public Image contents;
   // public float lerpspd;

    public Canvas canvas;
    GraphicRaycaster GR;
    PointerEventData PED;

    GameObject TargetA; //분리될 Doubling 기어 (큰 쪽)
    GameObject TargetB; //분리될 Doubling 기어 (작은 쪽)


    public GameObject SpdChecker;
    public Text spdtext;

    void Start()
    {
       // rectTransform = GetComponent<RectTransform>(); //게이지 UI의 recttransform

        GR = canvas.GetComponent<GraphicRaycaster>();
        PED = new PointerEventData(null);

        
    }

    // Update is called once per frame
    void Update()
    {

        this.gameObject.transform.position = ConvertCameraSpace(Input.mousePosition);

        if (Input.GetMouseButton(1)) //우클릭 꾸욱
        {
            if (SpdChecker.activeSelf == false)
            {
                SpdChecker.SetActive(true);
                //this.gameObject.transform.position = Input.mousePosition;
            }
            GetGearInfo();

        }
        else
        {
            if (SpdChecker.activeSelf == true)
            {
                SpdChecker.SetActive(false);
            }

        }



    }

    void GetGearInfo()
    {
        PED.position =Input.mousePosition;
        List<RaycastResult> GRresult = new List<RaycastResult>();
        GR.Raycast(PED, GRresult);

        if (GRresult.Count > 0)
        {
            for (int i = 0; i < GRresult.Count; i++)
            {
                if (GRresult[i].gameObject.transform.parent.CompareTag("Gear"))
                {
                    //Debug.Log(GRresult[i]);
                    Gear SelectedGear = GRresult[i].gameObject.transform.parent.GetComponent<Gear>();
                    spdtext.text = SelectedGear.rotate_speed.ToString();
                    break;
                }
                else
                {
                    spdtext.text = "-";

                }
            }


        }


    }

    public Vector3 ConvertCameraSpace(Vector3 ori)
    {
        Vector3 con = new Vector3(ori.x, ori.y, 100f); //Canvas의 Plane Distance 값을 z 축에 넣어주기
        return Camera.main.ScreenToWorldPoint(con);
    }
}
