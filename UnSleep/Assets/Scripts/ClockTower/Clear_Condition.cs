using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clear_Condition : MonoBehaviour
{

    public GameObject[] Gear_Object;
    public PuzzleClear puzzleClear;
    private int CurEpiId = 0;

    private float ClearCount = 0f;
    private bool isClearCondition = false;
    public Image blinkPanel;
    private bool tictokisPlaying;
    private float time;

    Collider2D col;
    public GameObject pointA, pointB;
    public GameObject ClearUI;


    private void OnEnable()
    {
    }


    private void Start()
    {
        CurEpiId = Dialogue_Proceeder.instance.CurrentEpiID;
    }

    void Update()
    {
        if (isClearCondition)
        {
            ClearCount += 1f * Time.deltaTime;
            if (!tictokisPlaying)
                StartCoroutine("TicTok");
        }
        else
        {
            ClearCount = 0f;
            tictokisPlaying = false;
            StopCoroutine("TicTok");
            blinkPanel.color = new Color(1f, 1f, 1f, 0f);
        }

        if (CurEpiId == 4) //18세 시험
        {
            if (CounterClockwise(Gear_Object[0]))
                isClearCondition = true;
            else
                isClearCondition = false;

        }
        else if (CurEpiId == 5) //19세 
        {
            if (Clockwise(Gear_Object[0]) && CounterClockwise(Gear_Object[1]))
                isClearCondition = true;
            else
                isClearCondition = false;


        }
        else if (CurEpiId == 8) //23세
        {
            if (!Dialogue_Proceeder.instance.AlreadyDone(81)) //23-1
                CheckArea();
            else //23-2
            {
                if (SpeedCheckDown(Gear_Object[0], 200f))
                    isClearCondition = true;
                else
                    isClearCondition = false;

            }
        }
        else if (CurEpiId == 9) //24세
        {
            if (CounterClockwise(Gear_Object[0]) && CounterClockwise(Gear_Object[1]) && Clockwise(Gear_Object[2]))
                isClearCondition = true;
            else
                isClearCondition = false;


        }
        else if (CurEpiId == 999) //삭제
        {
            if (CounterClockwise(Gear_Object[0]) && CounterClockwise(Gear_Object[1])
                && Clockwise(Gear_Object[2]) && Clockwise(Gear_Object[3]))
                isClearCondition = true;
            else
                isClearCondition = false;
        }
        else if (CurEpiId == 12) //31세 
        {
            if (SpeedCheck(Gear_Object[0], 100f))
                isClearCondition = true;
            else
                isClearCondition = false;
        }
        else if (CurEpiId == 13) //32세
        {
            if (Clockwise(Gear_Object[0]) && CounterClockwise(Gear_Object[1]) && SpeedCheck(Gear_Object[2], 20f))
                isClearCondition = true;
            else
                isClearCondition = false;

        }
        else if (CurEpiId == 14) //45세
        {
            if (SpeedCheck(Gear_Object[0], 100f) && SpeedCheckDown(Gear_Object[1], 40f))
                isClearCondition = true;
            else
                isClearCondition = false;
        }
        else if (CurEpiId == 19) //잘 있어요 -1
        {
            if (SpeedCheck(Gear_Object[0], 8f) && Clockwise(Gear_Object[0]) && CounterClockwise(Gear_Object[1]))
                isClearCondition = true;
            else
                isClearCondition = false;
        }
        else if (CurEpiId == 192) //잘 있어요 -2
        {
            if (SpeedCheck(Gear_Object[0], 100f) && Clockwise(Gear_Object[0]))
                isClearCondition = true;
            else
                isClearCondition = false;
        }

        if (ClearCount >= 3.5f)
            Clear();

    }

    public void Clear()
    {
        if (CurEpiId == 8 && !Dialogue_Proceeder.instance.AlreadyDone(81)) //23-1 클리어 시
        {
            Dialogue_Proceeder.instance.AddCompleteCondition(81);
            ClearUI.SetActive(true);
            ClearCount = 0f; 
            isClearCondition = false;
        }
        else
        {
            puzzleClear.ClearPuzzle(SceneType.MenTal, 0f);

        }
    }

    public void resetClear() //2개 이상의 스테이지가 나오는 경우 클리어를 초기화
    {
        ClearCount = 0f;
        //Clear_UI.SetActive(false);
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
                isClearCondition = true;
            else
                isClearCondition = false;
        }
        else
        {
            isClearCondition = false;
        }
    }

    IEnumerator TicTok()
    {

        SoundManager.Instance.PlaySE("tictokshort");
        time = 0f;
        tictokisPlaying = true;
        
        Color color = blinkPanel.color;
        color.a = Mathf.Lerp(0f, 0.4f, time);

        while(color.a < 0.4f)
        {
            time += Time.deltaTime / 0.5f;
            color.a = Mathf.Lerp(0f, 0.4f, time);
            blinkPanel.color = color;

            yield return null;

        }
        time = 0f;

        while (color.a > 0f)
        {
            time += Time.deltaTime / 0.5f;
            color.a = Mathf.Lerp(0.4f, 0f, time);
            blinkPanel.color = color;

            yield return null;

        }


        tictokisPlaying = false;
        yield return null;
    }
 
}
