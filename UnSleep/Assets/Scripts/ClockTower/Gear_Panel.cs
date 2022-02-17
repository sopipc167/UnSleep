using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gear_Panel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //bool Open = false;
    //bool Moving = false;
    Vector3 OpenPos;
    Vector3 ClosePos;
    
    

    void Start()
    {
        ClosePos = transform.position;
        OpenPos = new Vector3(ClosePos.x, ClosePos.y + 220f, ClosePos.z);
    }


    public void OnTriggerStay2D(Collider2D other)
    {
        Gear_Drag gear_drag = other.gameObject.GetComponent<Gear_Drag>();
        if (!gear_drag.Draging && gear_drag.Reset)
            gear_drag.GoToStartPos();

    }


    public void OnPointerEnter(PointerEventData eventData)
    {
       // if (!Moving && !Open)
       // {
       //     StartCoroutine(Open_panel());
       // }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //if (!Moving && Open)
       // {
        //    StartCoroutine(Close_panel());
        //}
    }



    /*
     
         IEnumerator Open_panel()
    {
        Moving = true;
        while (Vector3.Distance(transform.position,OpenPos ) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, OpenPos, 600f*Time.deltaTime);
            yield return null;
        }
        Moving = false;
        Open = true;
        //안돌도록 나중에 처리

    }

    IEnumerator Close_panel()
    {
        Moving = true;
        while (Vector3.Distance(transform.position, ClosePos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, ClosePos, 500f* Time.deltaTime);
            yield return null;
        }
        Moving = false;
        Open = false;
    }

     
     */


}
