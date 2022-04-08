using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clear_Condition : MonoBehaviour
{

    public GameObject[] Gear_Object;
    public GameObject Clear_UI;
    public int Level = 0;

    public float ClearCount = 0f;

    Collider2D col;
    public GameObject pointA, pointB;


    private void OnEnable()
    {
        if (Clear_UI.activeSelf) //클리어 UI가 켜져있으면 끄기
            Clear_UI.SetActive(false);
    }

    

    void Update()
    {
        if (Level == 0)
        {
            if (CounterClockwise(Gear_Object[0]))
                ClearCount += 1f*Time.deltaTime;
            else
                ClearCount = 0f;

        }
        else if (Level == 1)
        {
            if (Clockwise(Gear_Object[0]) && CounterClockwise(Gear_Object[1]))
                ClearCount += 1f * Time.deltaTime;
            else
                ClearCount = 0f;


        }
        else if (Level == 2)
        {
            CheckArea();
            //if (SpeedCheck(Gear_Object[0], 300f))
            //    ClearCount += 1f;
            //else
             //   ClearCount = 0f;



        }
        else if (Level == 3)
        {
            if (SpeedCheckDown(Gear_Object[0], 200f))
                ClearCount += 1f * Time.deltaTime;
            else
                ClearCount = 0f;



        }
        else if (Level == 4)
        {
            if (CounterClockwise(Gear_Object[0]) && CounterClockwise(Gear_Object[1]) && Clockwise(Gear_Object[2]))
                ClearCount += 1f * Time.deltaTime;
            else
                ClearCount = 0f;


        }
        else if (Level == 5)
        {
            if (CounterClockwise(Gear_Object[0]) && CounterClockwise(Gear_Object[1])
                && Clockwise(Gear_Object[2]) && Clockwise(Gear_Object[3]))
                ClearCount += 1f * Time.deltaTime;
            else
                ClearCount = 0f;
        }
        else if (Level == 6)
        {
            if (SpeedCheck(Gear_Object[0], 100f))
                ClearCount += 1f * Time.deltaTime;
            else
                ClearCount = 0f;
        }
        else if (Level == 7)
        {
            if (Clockwise(Gear_Object[0]) && CounterClockwise(Gear_Object[1]) && SpeedCheck(Gear_Object[2], 20f))
                ClearCount += 1f * Time.deltaTime;
            else
                ClearCount = 0f;

        }
        else if (Level == 8)
        {
            if (SpeedCheck(Gear_Object[0], 100f) && SpeedCheckDown(Gear_Object[1], 40f))
                ClearCount += 1f * Time.deltaTime;
            else
                ClearCount = 0f;
        }
        else if (Level == 9)
        {
            if (SpeedCheck(Gear_Object[0], 8f) && Clockwise(Gear_Object[0]) && CounterClockwise(Gear_Object[1]))
                ClearCount += 1f * Time.deltaTime;
            else
                ClearCount = 0f;
        }
        else if (Level == 10)
        {
            if (SpeedCheck(Gear_Object[0], 100f) && Clockwise(Gear_Object[0]))
                ClearCount += 1f * Time.deltaTime;
            else
                ClearCount = 0f;
        }

        if (ClearCount >= 3f)
            Clear();

    }

    public void Clear()
    {
        Clear_UI.SetActive(true);
        //Dialogue_Proceeder.instance.AddCompleteCondition(Complete_Condition);
        //Dialogue_Proceeder.instance.UpdateCurrentDiaID(Complete_After_Diaid);

    }

    public void resetClear() //2개 이상의 스테이지가 나오는 경우 클리어를 초기화
    {
        ClearCount = 0f;
        Clear_UI.SetActive(false);
    }

    bool Clockwise(GameObject Static_Gear)
    {
        if (Static_Gear.GetComponent<Gear>().Operating && Static_Gear.GetComponent<Gear>().rotation == -1)
            return true;

        return false;
    }

    bool CounterClockwise(GameObject Static_Gear)
    {
        if (Static_Gear.GetComponent<Gear>().Operating && Static_Gear.GetComponent<Gear>().rotation == 1)
            return true;

        return false;
    }

    bool SpeedCheck(GameObject Static_Gear, float speed)
    {
        if (Static_Gear.GetComponent<Gear>().rotate_speed == speed)
            return true;

        return false;
    }

    bool SpeedCheckDown(GameObject Static_Gear, float speed)
    {
        if (Static_Gear.GetComponent<Gear>().rotate_speed <= speed && Static_Gear.GetComponent<Gear>().rotate_speed>0f)
            return true;

        return false;
    }

    bool SpeedCheckUp(GameObject Static_Gear, float speed)
    {
        if (Static_Gear.GetComponent<Gear>().rotate_speed >= speed && Static_Gear.GetComponent<Gear>().rotate_speed > 0f)
            return true;

        return false;
    }

    void CheckArea()
    {
        col = Physics2D.OverlapArea(pointA.transform.position, pointB.transform.position); //주변에 있는 콜라이더 인식
        if (col != null)
        {
            if (col.gameObject.GetComponent<Gear>().teeth_num == 10)
                ClearCount += 1f * Time.deltaTime;
            else
                ClearCount = 0f;
        }
        else
        {
            ClearCount = 0f;
        }
    }

 
}
