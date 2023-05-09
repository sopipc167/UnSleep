using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class GearUI65 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler
{
    private Vector3 StartPos;
    private Vector3 CenterPos = new Vector3(0, 0, 0);
    Vector3 offset;
    private float StartDistance;
    private RectTransform rect;

    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        StartPos = rect.position;
        StartDistance = Vector3.Distance(StartPos, CenterPos);
    }

    void Update()
    {
        float CurScale = 0.8f + 0.5f / Vector3.Distance(rect.position, CenterPos);
        rect.localScale = new Vector3(CurScale, CurScale);

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

    public void OnScroll(PointerEventData eventData)
    {
        Quaternion curQua = rect.rotation;
        Quaternion newQua = Quaternion.Euler(new Vector3(0, 0, 60 * eventData.scrollDelta.y));
        rect.DORotateQuaternion(curQua*newQua, 0.5f);
    }

}
