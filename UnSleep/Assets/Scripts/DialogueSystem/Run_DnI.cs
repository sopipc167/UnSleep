using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run_DnI : MonoBehaviour
{
    public GameObject[] DnI_List;
    public GameObject DnI_Parent; //Canvas 내 순서 고정을 위한 DnI 부모 오브젝트

    private InteractManager interactManager;

    private void Start()
    {
        interactManager = GetComponent<InteractManager>();
    }

    public void Run_Direc_N_Inter()
    {
        GameObject DnI = Instantiate(DnI_List[GetIdx()]);
        DnI.transform.position = DnI_Parent.transform.position;
        DnI.transform.SetParent(DnI_Parent.transform);

        //현재 상호작용을 가져와서 시작
        interactManager.StartInteraction(DnI.GetComponent<StoryInteract>());
    }

    private int GetIdx()
    {
        switch (Dialogue_Proceeder.instance.CurrentDiaID)
        {
            case 3201: return 0;
            case 3116: return 1;
            case 3117: return 2;
            case 5602: return 3;
            case 6502: return 4;
            case 6517: return 5;
        }
        return -1;
    }
}
