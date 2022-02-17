using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SetLogContent : MonoBehaviour, IPointerClickHandler
{
    public Image char_img; //초상화
    public Text name_; //이름
    public Text context; //대사
    public int Dia_id; //대화묶음 id
    public int dialogues_idx; //대사 번호 id


    BackToLogSave backtologsave;

    void Update()
    {
        transform.localScale = Vector3.one;
    }

    public void Set(Sprite sprite, string n, string c,int Did, int diaidx)
    {
        char_img.sprite = sprite;
        name_.text = n;
        context.text = c;
        Dia_id = Did;
        dialogues_idx = diaidx;

    }

    public void Set_narration(string c, int Did, int diaidx) //나레이션이면 대사만 출력
    {
        char_img.sprite = null;
        name_.text = "";
        context.text = c;
        Dia_id = Did;
        dialogues_idx = diaidx;

    }

    public void OnPointerClick(PointerEventData eventData) //로그를 클릭하면 일단 정보 보내줌
    {
        backtologsave = GameObject.Find("Log").GetComponent<BackToLogSave>();
        backtologsave.BackToSeletedLog(Dia_id, dialogues_idx);

    }


}
