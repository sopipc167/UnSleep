using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DrawLine : MonoBehaviour
{
    [Header("참조")]
    public Transform lineGroup;
    public GameObject linePrefab;
    public Slider memoSlider;
    public Button[] colorButtons;
    private Color[] colors;

    [Header("그려지는 선 옵션")]
    [Range(0.01f, 0.1f)]
    public float lineWidth;

    private int colorIdx = 0;
    private MemoManager memoManager;
    private EraseLine eraseLine;
    private LineRenderer lineRenderer;
    private Vector2 oldPos;

    private void Start()
    {
        memoManager = GetComponent<MemoManager>();
        eraseLine = GetComponent<EraseLine>();
        memoSlider.maxValue = 0.2f;
        memoSlider.minValue = 0.05f;
        memoSlider.value = lineWidth;
        memoManager.deleteAllButton.onClick.AddListener(OnClickDeleteAllButton);
        colors = new Color[colorButtons.Length];
        for (int i = 0; i < colorButtons.Length; ++i)
        {
            int index = i;
            colorButtons[i].onClick.AddListener(() => OnClickColorButton(index));
            colors[i] = colorButtons[i].image.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (MemoManager.isMemoOn && !memoManager.isEraseMode)
        {
            memoManager.memoButton.Select();
            if (!ExceptUIClick.isActive)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (memoManager.memoDetailCanvas.activeSelf)
                    {
                        memoManager.memoDetailCanvas.SetActive(false);
                    }

                    GameObject obj = Instantiate(linePrefab, lineGroup);
                    obj.GetComponent<MemoLine>().erase = eraseLine;
                    lineRenderer = obj.GetComponent<LineRenderer>();
                    lineRenderer.startColor = colors[colorIdx];
                    lineRenderer.endColor = colors[colorIdx];
                    lineRenderer.widthMultiplier = lineWidth;
                    lineRenderer.positionCount = 1;
                    oldPos = memoManager.memoCamera.ScreenToWorldPoint(Input.mousePosition);
                    lineRenderer.SetPosition(0, oldPos);
                }
                else if (Input.GetMouseButton(0))
                {
                    Vector2 pos = memoManager.memoCamera.ScreenToWorldPoint(Input.mousePosition);
                    if (Vector2.Distance(oldPos, pos) > 0.1f)
                    {
                        oldPos = pos;
                        ++lineRenderer.positionCount;
                        lineRenderer.SetPosition(lineRenderer.positionCount - 1, pos);
                    }
                }
            }
            else
            {
                lineWidth = memoSlider.value;
            }
        }
    }

    private void OnClickDeleteAllButton()
    {
        memoManager.memoDetailCanvas.SetActive(false);
        int size = lineGroup.transform.childCount;
        if (size == 0) return;

        for (int i = size - 1; i >= 0; i--)
        {
            Destroy(lineGroup.transform.GetChild(i).gameObject);
        }
    }

    private void OnClickColorButton(int idx)
    {
        colorIdx = idx;
    }
}
