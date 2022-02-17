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

        this.gameObject.transform.position = Input.mousePosition;

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


        /*
         오른쪽 클릭 꾹 누르면 무언가 진행했던 코드
                if (Input.GetMouseButton(1)) //우클릭 꾸욱
        {
            if (!UnDoDoubling)
            {
                if (RightDowngage.activeSelf == false) //게이지 꺼져있으면
                {
                    RightDowngage.SetActive(true);
                    this.gameObject.transform.position = Input.mousePosition; //켜서 마우스 옆에 띄우고
                }
                curRightDown += 1f * Time.deltaTime; //카운트

            }
        }
        else //우클릭 x
        {
            if (RightDowngage.activeSelf == true) //켜져 있으면 끄고
                RightDowngage.SetActive(false);
            curRightDown = 0f; //초기화
        }

        if (curRightDown <= maxRightDown) //꾹 누르는 중
        {
            contents.fillAmount = curRightDown / maxRightDown;
        }
        else //지정한 시간만큼 누르면?
        {
            if (RightDowngage.activeSelf == true) //켜져 있으면 끄고
                RightDowngage.SetActive(false);
            curRightDown = 0f; //초기화

            UnDoDoubling = true;
            GRDoublingGear();
        }


   


         */


    }

    void GetGearInfo()
    {
        PED.position = Input.mousePosition;
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

    /*
         void GRDoublingGear()
    {
        PED.position = Input.mousePosition;
        List<RaycastResult> GRresult = new List<RaycastResult>();
        GR.Raycast(PED, GRresult);

        if (GRresult.Count > 0)
        {
            for (int i = 0; i < GRresult.Count; i++)
            {
                if(GRresult[i].gameObject.transform.parent.CompareTag("Gear"))
                {
                    GameObject SelectedGear = GRresult[i].gameObject.transform.parent.gameObject;
                    GameObject SGwithDoubling = SelectedGear.GetComponent<Gear>().DoublingGear.gameObject;

                    if (SelectedGear.GetComponent<Gear>().teeth_num > SGwithDoubling.GetComponent<Gear>().teeth_num)
                        SelectedGear.GetComponent<Gear>().UnDoGearDoubling(SelectedGear, SGwithDoubling);
                    else
                        SelectedGear.GetComponent<Gear>().UnDoGearDoubling(SGwithDoubling, SelectedGear);


                    break;
                }
            }


        }

    }

     */

}
