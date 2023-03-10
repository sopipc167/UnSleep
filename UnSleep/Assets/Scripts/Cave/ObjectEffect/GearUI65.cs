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

    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.position;
        StartDistance = Vector3.Distance(StartPos, CenterPos);
    }

    void Update()
    {
        float CurScale = (StartDistance / Vector3.Distance(transform.position, CenterPos)) * -0.7f + 1.5f;
        transform.localScale = new Vector3(CurScale, CurScale);

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Vector3 CurPos = Input.mousePosition;
            transform.position = CurPos + offset;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            offset = transform.position - Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.DOMove(StartPos, 1f, true);
    }

    public void OnScroll(PointerEventData eventData)
    {
        Quaternion curQua = transform.rotation;
        Quaternion newQua = Quaternion.Euler(new Vector3(0, 0, 60 * eventData.scrollDelta.y));
        transform.DORotateQuaternion(curQua*newQua, 0.5f);
    }
}
