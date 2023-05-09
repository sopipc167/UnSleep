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
    public float typingInterval = 0.02f;
    private bool isTyping;

    private PlayableDirector playableDirector;
    private TextManager textManager;
    private Camera mainCamera; // 끌 때 지정해두지 않으면 다시 킬 때 못찾음

    
   
    void OnEnable()
    {
        textManager = GameObject.Find("Canvas").transform.GetChild(0).transform.GetChild(0).GetComponent<TextManager>();
        playableDirector = GetComponent<PlayableDirector>();
        playableDirector.Play();
        mainCamera = Camera.main.GetComponent<Camera>();
        mainCamera.enabled = false;
        StartCoroutine(printLineCine());
    }


    IEnumerator printLineCine()
    {
        for (int i = 0; i < LineList.Length;)
        {
            if (!isTyping)
            {
                StartCoroutine(OnType(typingInterval, LineList[i]));
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
        mainCamera.enabled = true;
        Dialogue_Proceeder.instance.UpdateCurrentDiaIDPlus1();
        textManager.SetDiaInMap();
        this.gameObject.SetActive(false);
    }

}
