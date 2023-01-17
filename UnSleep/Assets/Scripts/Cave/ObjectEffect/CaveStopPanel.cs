using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CaveStopPanel : MonoBehaviour, IPointerClickHandler
{

    private bool showDiaUI = true;
    public GameObject DiaUI;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (showDiaUI)
            DiaUI.SetActive(!DiaUI.activeSelf);
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
