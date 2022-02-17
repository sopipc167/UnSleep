using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EraseLine : MonoBehaviour
{
    [Header("참조")]
    public Slider eraseSlider;

    [Header("지우는 영역 설정")]
    public int vertexCount = 40;
    [Range(0.01f, 0.1f)]
    public float lineWidth;
    public Color color;

    //for MemoLine
    internal bool isErasing = false;
    internal float radius = 0f;
    internal Vector3 mousePos = Vector3.zero;

    private Vector3 oldPos = Vector3.zero;
    private MemoManager memoManager;
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        memoManager = GetComponent<MemoManager>();
        lineRenderer = memoManager.eraseButton.GetComponent<LineRenderer>();
        lineRenderer.widthMultiplier = lineWidth;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        eraseSlider.value = radius;
        eraseSlider.minValue = 0.5f;
        eraseSlider.maxValue = 1.5f;
    }

    private void Update()
    {
        if (MemoManager.isMemoOn && memoManager.isEraseMode)
        {
            memoManager.eraseButton.Select();
            radius = eraseSlider.value;

            mousePos = memoManager.memoCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            if (oldPos != mousePos)
            {
                ShowEraseArea(mousePos, radius);
                oldPos = mousePos;
            }

            if (!ExceptUIClick.isActive && Input.GetMouseButton(0))
            {
                isErasing = true;
                if (memoManager.eraseDetailCanvas.activeSelf)
                {
                    memoManager.eraseDetailCanvas.SetActive(false);
                }
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                isErasing = false;
            }
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }

    public void ShowEraseArea(Vector3 center, float radius)
    {
        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;

        lineRenderer.positionCount = vertexCount;
        for (int i = 0; i < vertexCount; i++)
        {
            Vector3 pos = center + new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f);
            lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }
}
