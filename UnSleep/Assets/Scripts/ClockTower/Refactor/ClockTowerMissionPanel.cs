using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClockTowerMissionPanel : MonoBehaviour, IPointerEnterHandler
{
    private GameObject text;
    private RectTransform rect;

    public bool isFold = false;
    public bool isMoving = false;
    private Vector3 openPos = new Vector3(-360f, 465f, 0f);
    private Vector3 foldPos = new Vector3(-1350f, 465f, 0f);

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isMoving)
        {
            text.SetActive(isFold);
            isFold = !isFold;
        }

    }

    void Start()
    {
        text = transform.GetChild(0).gameObject;
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {

        if (isFold) // 접힘
        {
            rect.anchoredPosition = Vector3.Lerp(rect.anchoredPosition, foldPos, Time.deltaTime * 5f);
            isMoving = Vector3.Distance(rect.anchoredPosition, foldPos) > 1f; 
        } else // 펼침
        {
            rect.anchoredPosition = Vector3.Lerp(rect.anchoredPosition, openPos, Time.deltaTime * 5f);
            isMoving = Vector3.Distance(rect.anchoredPosition, openPos) > 1f;
        }

    }
}
