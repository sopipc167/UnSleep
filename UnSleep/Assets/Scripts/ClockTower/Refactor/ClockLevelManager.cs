using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockLevelManager : MonoBehaviour
{
    public PuzzleClear puzzleClear;
    public Image blinkPanel;
    public GameObject clearUI;

    private BCogWheel[] bCogWheels;
    private float clearCount = 0f;
    private bool tiktoking = false;
    private Coroutine coroutine;


    void Start()
    {
        bCogWheels = FindObjectsOfType<BCogWheel>();
    }

    private void checkClear()
    {
        if (bCogWheels.Count(bcw => !bcw.satisfy) == 0)
        {
            clearCount += 1f * Time.deltaTime;
            if (!tiktoking) 
                coroutine = StartCoroutine(TicTok());
        } else
        {
            clearCount = 0f;
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }

        }
    }


    void Update()
    {
        checkClear();

        if (clearCount >= 4f) clear();
    }
    public void clear()
    {
        if (Dialogue_Proceeder.instance.CurrentEpiID == 8 && !Dialogue_Proceeder.instance.AlreadyDone(81)) //23-1 클리어 시
        {
            Dialogue_Proceeder.instance.AddCompleteCondition(81);
            clearUI.SetActive(true);
            clearCount = 0f;
        }
        else if (Dialogue_Proceeder.instance.CurrentEpiID == 19 && Dialogue_Proceeder.instance.CurrentDiaID == 8036) //잘있어요-1 클리어 시
        {
            Dialogue_Proceeder.instance.AddCompleteCondition(31);
            clearUI.SetActive(true);
            // textManager.Set_Dialogue_Goodbye();
            clearCount = 0f;
        }
        else if (Dialogue_Proceeder.instance.CurrentEpiID == 19 && Dialogue_Proceeder.instance.CurrentDiaID == 8037)
        {
            Dialogue_Proceeder.instance.AddCompleteCondition(32);
            clearUI.SetActive(true);
            // textManager.Set_Dialogue_Goodbye();
            clearCount = 0f;
        }
        else
        {
            puzzleClear.ClearPuzzle(SceneType.Mental, 0f);
            SoundManager.Instance.FadeOutBGM();
        }
    }

    IEnumerator TicTok()
    {

        SoundManager.Instance.PlaySE("tictokshort");
        float time = 0f;
        tiktoking = true;

        Color color = blinkPanel.color;
        color.a = Mathf.Lerp(0f, 0.4f, time);

        while (color.a < 0.4f)
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


        tiktoking = false;
        yield return null;
    }
}
