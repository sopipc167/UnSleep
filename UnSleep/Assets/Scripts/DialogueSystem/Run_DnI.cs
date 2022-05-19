using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run_DnI : MonoBehaviour
{
    public GameObject[] DnI_List;
    public GameObject DnI_Parent; //Canvas 내 순서 고정을 위한 DnI 부모 오브젝트

    private InteractManager interactManager;
    private int prevIdx;
    private GameObject curDnI;

    private void Start()
    {
        interactManager = GetComponent<InteractManager>();
    }

    public void Run_Direc_N_Inter()
    {
        int curIdx = GetIdx();
        if (curIdx != prevIdx)
        {
            curDnI = Instantiate(DnI_List[GetIdx()]);
            prevIdx = curIdx;
        }
        else
        {
            curDnI.SetActive(true);
        }
        curDnI.transform.position = DnI_Parent.transform.position;
        curDnI.transform.SetParent(DnI_Parent.transform);

        //현재 상호작용을 가져와서 시작
        interactManager.StartInteraction(curDnI.GetComponent<StoryInteract>());
    }

    private int GetIdx()
    {
        switch (Dialogue_Proceeder.instance.CurrentDiaID)
        {
            case 2304: return 0;    // 세나
            case 2403: return 1;    // 도문
            case 3108: return 2;    // 엘범
            case 3116: return 3;    // 메시지1
            case 3117: return 4;    // 메시지2
            case 3201: return 5;    // 맥주1
            case 4506: return 6;    // event45
            case 5602: return 7;    // 메시지3
            case 6502: return 8;    // 맥주2
            case 6517: return 9;    // 리본
        }
        return -1;
    }
}
