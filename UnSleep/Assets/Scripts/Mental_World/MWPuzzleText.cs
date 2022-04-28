using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class MWPuzzleText : MonoBehaviour
{

    [Header("시네마틱 대사 재생")]
    public Text LineText;
    public string[] LineList;
    public float interval_ = 2f;
    private bool isTyping;

    private PlayableDirector playableDirector;
    private TextManager textManager;

    
   
    void OnEnable()
    {
        textManager = GameObject.Find("Canvas").transform.GetChild(0).transform.GetChild(0).GetComponent<TextManager>();
        playableDirector = GetComponent<PlayableDirector>();
        playableDirector.Play();
        StartCoroutine(printLineCine());
    }


    IEnumerator printLineCine()
    {
        for (int i = 0; i < LineList.Length;)
        {
            if (!isTyping)
            {
                StartCoroutine(OnType(0.05f, LineList[i]));
                i++;
            }

            yield return new WaitForSeconds(interval_);
        }
    }

    IEnumerator OnType(float interval, string Line)
    {
        isTyping = true;
        LineText.text = "";

        foreach (char item in Line)
        {
            LineText.text += item;
            yield return new WaitForSeconds(interval);
        }
        isTyping = false;
    }

    public void DisableMWPuzzle()
    {
        Dialogue_Proceeder.instance.UpdateCurrentDiaIDPlus1();
        textManager.SetDiaInMap();
        this.gameObject.SetActive(false);
    }

}
