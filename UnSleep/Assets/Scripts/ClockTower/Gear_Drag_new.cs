﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Gear_Drag_new : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    public Vector3 ScrSpace;
    Vector3 Start_pos; //시작위치
    Vector3 Before_pos; //직전위치
    Vector3 offset;
    public bool Moving = false; //톱니 이동중
    public bool Draging; //드래깅중
    public bool Reset = false;
    public bool Stop = false; //true가 되면 마지막 연출 실행 
    GameObject Main_Panel;
    GameObject Gear_Panel;


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

        if (Stop)
            return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ScrSpace = transform.parent.transform.position;
            offset = transform.parent.transform.position - new Vector3(Input.mousePosition.x, Input.mousePosition.y, ScrSpace.z);
            Draging = true;
            clkSound.PlaySound(0);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Stop)
            return;


        if (eventData.button == PointerEventData.InputButton.Left)
        {
            transform.parent.transform.SetParent(Main_Panel.transform);
            Vector3 curScrSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, ScrSpace.z);
            Vector3 curPosition = curScrSpace + offset;
            transform.parent.transform.position = curPosition;
            Draging = true;

        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Stop)
            return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {

            Before_pos = transform.transform.parent.transform.position;
            

            clkSound.PlaySound(1);

        }
        Draging = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Stop)
            return;

        transform.GetChild(0).GetComponent<Outline>().enabled = true;
        //StartCoroutine("GetBigger");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OffEffect();
    }

    public void OffEffect()
    {
        transform.GetChild(0).GetComponent<Outline>().enabled = false;
        //transform.GetChild(0).transform.localScale = Vector3.one;

    }

}
