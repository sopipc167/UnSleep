using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CliffLine : MonoBehaviour
{
    private Camera mainCamera;
    private LineRenderer lineRenderer;

    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 mousePos;
    private Vector3 mouseDir;

    private Transform targetPos;
    private int InteractNum = 0;
    private bool isStart = false;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.numCornerVertices = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseDir = mousePos - targetPos.position;
            mouseDir.z = 0;
            mouseDir = mouseDir.normalized;

            if (Input.GetMouseButton(0))
            {
                endPos = mousePos;
                endPos.z = 0f;
                lineRenderer.SetPosition(InteractNum, endPos);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                --lineRenderer.positionCount;
            }
        }
    }

    public void DrawLine(Transform target)
    {
        targetPos = target;
        startPos = target.position;
        startPos.z = 10f;

        lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPos);
        InteractNum = 1;
        isStart = true;
    }

    public void SetPoint(Transform target)
    {
        targetPos = target;
        startPos = target.position;
        if (InteractNum % 2 != 0)
        {
            startPos.z = 40f;
        }
        else
        {
            startPos.z = 10f;
        }

        ++lineRenderer.positionCount;
        lineRenderer.SetPosition(InteractNum, startPos);
        ++InteractNum;
    }

    public void StopDrawing()
    {
        lineRenderer.enabled = false;
        isStart = false;
    }
}
