using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CogWheelInfoPanel : MonoBehaviour
{
    public Text infoText;
    public GameObject infoPanel;

    private RectTransform rect;
    private float offsetX = 120f;
    private float offsetY = 80f;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        Debug.Log(Input.mousePosition);
        if (Input.mousePosition.y - offsetY < 10f)
        {
            rect.anchoredPosition = new Vector3(
              Input.mousePosition.x + offsetX,
              Input.mousePosition.y + offsetY,
              0f);
        } else
        {
            rect.anchoredPosition = new Vector3(
              Input.mousePosition.x + offsetX,
              Input.mousePosition.y - offsetY,
              0f);
        }

    }

    public void showInfo(CogWheelInfo info)
    {
        if (!infoPanel.activeSelf)
        {
            infoPanel.SetActive(true);
        }

        infoText.text = string.Format("{0}\n{1}", info.size, (int)info.speed);
    }

    public void hideInfo()
    {
        infoText.text = "";
        infoPanel.SetActive(false);
    }
}
