using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Gear_Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{

    public Vector3 ScrSpace;
    Vector3 Start_pos; //시작위치
    Vector3 Before_pos; //직전위치
    Vector3 offset;
    public bool Moving = false; //톱니 이동중
    public bool Draging; //드래깅중
    public bool Reset = false;
    GameObject Main_Panel;
    GameObject Gear_Panel;

    private float effecttime = 0.5f;

    ClkSound clkSound;

    void Start()
    {
        Main_Panel = GameObject.Find("Main_Panel");
        Gear_Panel = GameObject.Find("Gear_Panel");

        Start_pos = transform.parent.transform.position;
        clkSound = GameObject.Find("EtcManager").GetComponent<ClkSound>();
    }

    void Update()
    {
        if (transform.parent.transform.position != Start_pos)
            Reset = true;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Moving)
            return;

        OffEffect();
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            offset = transform.parent.transform.position - ConvertCameraSpace(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100f));



            Draging = true;
            clkSound.PlaySound(0);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            transform.parent.transform.SetParent(Main_Panel.transform);
            Vector3 curScrSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, ScrSpace.z);
            Vector3 curPosition = ConvertCameraSpace(curScrSpace + offset);
            transform.parent.transform.position = curPosition;
            Draging = true;

        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Left)
        {


            if (!GetComponent<Gear>().RotateDirection) //나중에 수정해야 할듯?
            {
                transform.SetParent(GetComponent<Gear>().Origin_Parent.transform);
                transform.parent.transform.SetParent(Gear_Panel.transform);
                GetComponent<Gear>().in_Main_Panel = false;


                if (GetComponent<Gear>().Operating)
                    GetComponent<Gear>().Operating = false;
                GetComponent<Gear>().Gear_image.color = new Color(1f, 1f, 1f);
            }
            else
            {
                GetComponent<Gear>().in_Main_Panel = true;
                Before_pos = ConvertCameraSpace(transform.transform.parent.transform.position); 
            }

            clkSound.PlaySound(1);

        }
        Draging = false;
    }



    public void GoToStartPos()
    {

        if (Moving)
            return;
        Moving = true;
        StartCoroutine("BackToPos", Start_pos);
        transform.parent.transform.SetParent(Gear_Panel.transform);
        GetComponent<Gear>().in_Main_Panel = false;
        GetComponent<Gear>().False_Operating(this.gameObject);
        Moving = false;

    }

    IEnumerator BackToPos(Vector3 Target_Pos) //Target_Pos : Start_Pos, Before_Pos 올 수 있음
    {
        while (Vector3.Distance(transform.parent.transform.position, Target_Pos) > 0.05f)
        {
            transform.parent.transform.position = Vector3.MoveTowards(transform.parent.transform.position, Target_Pos, 20f);
            yield return null;
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.GetChild(0).GetComponent<Outline>().enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OffEffect();
    }



    public void OffEffect()
    {
        transform.GetChild(0).GetComponent<Outline>().enabled = false;
    }

  
    public Vector3 ConvertCameraSpace(Vector3 ori)
    {
        Vector3 con = new Vector3(ori.x, ori.y, 100f); //Canvas의 Plane Distance 값을 z 축에 넣어주기
        return Camera.main.ScreenToWorldPoint(con);
    }

}
