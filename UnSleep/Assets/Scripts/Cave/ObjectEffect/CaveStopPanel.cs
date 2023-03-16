using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CaveStopPanel : MonoBehaviour, IPointerClickHandler
{

    private bool showDiaUI = true;
    public GameObject DiaUI;
    private CaveMapManager caveMapManager;


    private void OnEnable()
    {
        if (caveMapManager != null)
        {
            caveMapManager.objectActive = true;
        }
        caveMapManager = GameObject.Find("CaveSystem").GetComponent<CaveMapManager>();

    }



    public void OnPointerClick(PointerEventData eventData)
    {
        if (showDiaUI)
        {
            DiaUI.SetActive(!DiaUI.activeSelf);
            
        }
    }


    public void dontShowDiaUI()
    {
        showDiaUI = false;
    }

    public void disableCaveStopPanel()
    {
        gameObject.SetActive(false);
    }
}
