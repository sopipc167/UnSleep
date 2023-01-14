using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CaveStopPanel : MonoBehaviour, IPointerClickHandler
{

    private bool showDiaUI = true;
    private GameObject DiaUI;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (showDiaUI)
            DiaUI.SetActive(!DiaUI.activeSelf);
    }

    private void Start()
    {
        DiaUI = transform.GetChild(0).gameObject;
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
