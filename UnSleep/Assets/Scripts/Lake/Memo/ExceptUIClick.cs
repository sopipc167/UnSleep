using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExceptUIClick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    static public bool isActive = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isActive = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isActive = false;
    }
}