using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SoundCompass : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform niddleTransfrom;

    private RectTransform rect;
    private Vector3 StartPos;
    private Vector3 CenterPos = new Vector3(0, 0, 0);
    Vector3 offset;

    public float zDir = 0f;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        StartPos = rect.position;

    }

    void Update()
    {
        float CurScale = 0.5f + 0.5f / Vector3.Distance(rect.position, CenterPos);
        rect.localScale = new Vector3(CurScale, CurScale);
        niddleTransfrom.rotation = Quaternion.Lerp(niddleTransfrom.rotation, Quaternion.Euler(0f, 0f, -zDir), Time.deltaTime) ;
    }


    public void updateSoundCompass(Cavern cavern)
    {
        float abs = 0f;
        float dir = 0f;

        switch (cavern.soundPosition)
        {
            case "LR":
                abs = 90f * cavern.volume2;
                dir = 1f;
                break;
            case "C":
                abs = 0f;
                dir = 0f;
                break;
            case "L":
                abs = 90f * cavern.volume;
                dir = -1f;
                break;
            case "R":
                abs = 90f * cavern.volume;
                dir = 1f;
                break;
            case "c":
                abs = 90f * cavern.volume;
                dir = 0f;
                break;
            case "l":
                abs = 90f * cavern.volume;
                dir = -1f;
                break;
            case "r":
                abs = 90f * cavern.volume;
                dir = 1f;
                break;
        }
        zDir = dir * abs;

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Vector3 CurPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rect.position = CurPos + offset;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            offset = rect.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        rect.DOMove(StartPos, 1f);
    }

}
