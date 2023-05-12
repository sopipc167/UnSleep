using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Epi6Direc : MonoBehaviour, DialogueDoneListener
{
    public GameObject mwpuzzle;
    // public FadeInOut fadeInOut; // 시네머신 시작 전 페이드 아웃
     public Image fadePanel; // 시네머신 시작 후 페이드 인 
    // 둘이 쓰는 카메라가 달라서 시네머신 위로 작동하는 UI 추가


    void Start()
    {
        mwpuzzle = GameObject.Find("Cinematic").transform.GetChild(1).gameObject;
        // fadeInOut = GameObject.Find("Canvas").transform.GetChild(1).GetComponent<FadeInOut>();
        fadePanel = mwpuzzle.transform.GetChild(7).GetChild(1).GetComponent<Image>();
        FindObjectOfType<TextManager>().addDialogueDoneListeners(this);

    }


    IEnumerator setMWPuzzleEnable()
    {
        Dialogue_Proceeder.instance.AddCompleteCondition(600);
        mwpuzzle.SetActive(true);

        for (float alpha = 1f; alpha > 0; alpha -= 0.05f)
        {
            fadePanel.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

    }

    public void OnDialogueEnd(int DiaId)
    {
        if (DiaId == 603 && !Dialogue_Proceeder.instance.AlreadyDone(600) && !mwpuzzle.activeSelf)
        {
            StartCoroutine(setMWPuzzleEnable());
        }
    }
}
