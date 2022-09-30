using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Epi6Direc : MonoBehaviour
{
    private GameObject mwpuzzle;
    private FadeInOut fadeInOut; // 시네머신 시작 전 페이드 아웃
    private Image fadePanel; // 시네머신 시작 후 페이드 인 
    // 둘이 쓰는 카메라가 달라서 시네머신 위로 작동하는 UI 추가


    void Start()
    {
        mwpuzzle = GameObject.Find("Cinematic").transform.GetChild(1).gameObject;
        fadeInOut = GameObject.Find("Canvas").transform.GetChild(1).GetComponent<FadeInOut>();
        fadePanel = mwpuzzle.transform.GetChild(6).GetChild(1).GetComponent<Image>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Dialogue_Proceeder.instance.AlreadyDone(603) && !Dialogue_Proceeder.instance.AlreadyDone(600) && !mwpuzzle.activeSelf)
        {
            StartCoroutine(setMWPuzzleEnable());
        }
     
    }

    IEnumerator setMWPuzzleEnable()
    {
        Dialogue_Proceeder.instance.AddCompleteCondition(600);
        fadeInOut.Fade_Out();

        yield return new WaitForSeconds(5f);

        
        mwpuzzle.SetActive(true);

        
        for (float alpha = 1f; alpha > 0; alpha -= 0.05f)
        {
            fadePanel.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

    }


}
