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

    public void Run_Direc_N_Inter(int index)
    {
        GameObject DnI = Instantiate(DnI_List[index]);
        DnI.transform.position = DnI_Parent.transform.position;
        DnI.transform.SetParent(DnI_Parent.transform);

        //현재 상호작용을 가져와서 시작
        interactManager.StartInteraction(DnI.GetComponent<StoryInteract>());
    }
}
