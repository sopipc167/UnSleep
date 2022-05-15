using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BeerImg : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    internal bool result;

    private Vector3 initPos;
    private Quaternion initRot;

    private Dialogue_Proceeder dp;
    private Vector3 curMousePos;
    private Vector3 prevMousePos;
    private Vector3 diff;
    private Image img;

    private void Awake()
    {
        dp = Dialogue_Proceeder.instance;
        img = GetComponent<Image>();
        initPos = transform.position;
        initRot = transform.rotation;
    }

    private void OnEnable()
    {
        result = false;
        img.raycastTarget = true;
        transform.SetPositionAndRotation(initPos, initRot);
    }

    private void Update()
    {
        if (!result) return;

        if (dp.CurrentDiaID == 3201 && dp.CurrentDiaIndex == 4 ||
            dp.CurrentDiaID == 6502 && dp.CurrentDiaIndex == 15)
        {
            transform.parent.gameObject.SetActive(false);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (result) return;

        prevMousePos = curMousePos;
        curMousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);

        diff = curMousePos - prevMousePos;

        if (transform.localPosition.x > -35.2f && diff.x > 0f)
        {
            diff.x = 0f;
        }

        if (transform.localPosition.y > -97.2f && diff.y > 0f)
        {
            diff.y = 0f;
        }

        // 재미로 넣은 코드...
        // 도문이 혼술 가능 ㅋㅋ
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, transform.localRotation.eulerAngles.z - (diff.x * 0.08f)));
        
        transform.localPosition += diff;

        if (transform.localPosition.x > -35.2f && transform.localPosition.y > -97.2f)
        {
            img.raycastTarget = false;
            result = true;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (result) return;
        curMousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
    }
}
