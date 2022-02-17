using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToLogSave : MonoBehaviour
{
    int Save_Dia_id;
    int Save_dialogue_idx;
    public GameObject BackToPopup;
    public GameObject Manager;

    public void BackToSeletedLog(int Dia_id, int dialogue_idx)
    {
        BackToPopup.SetActive(true);
        Save_Dia_id = Dia_id;
        Save_dialogue_idx = dialogue_idx;
    }

    public void Yes()
    {
        BackToPopup.SetActive(false);
        Manager.GetComponent<TextManager>().BackToSeletedLogYes(Save_Dia_id, Save_dialogue_idx);
    }

    public void No()
    {
        BackToPopup.SetActive(false);
    }

}
