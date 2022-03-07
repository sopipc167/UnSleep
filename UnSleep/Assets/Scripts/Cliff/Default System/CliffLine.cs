using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CliffLine : MonoBehaviour
{
    private Camera mainCamera;
    protected LineRenderer lineRenderer;

    protected Vector3 startPos;
    protected Vector3 endPos;
    private Vector3 mousePos;
    protected Vector3 mouseDir;

    protected Transform targetPos;
    protected int interactNum = 0;
    private bool isStart = false;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        mainCamera = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.numCornerVertices = 5;
    }

    // Update is called once per frame
    protected virtual void Update()
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
                lineRenderer.SetPosition(interactNum, endPos);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                --lineRenderer.positionCount;
            }
        }
    }

    public virtual void DrawLine(Transform target)
    {
        targetPos = target;
        startPos = target.position;
        startPos.z = 10f;

        lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPos);
        interactNum = 1;
        isStart = true;
    }

    public virtual void SetPoint(Transform target)
    {
        targetPos = target;
        startPos = target.position;
        if (interactNum % 2 != 0)
        {
            startPos.z = 40f;
        }
        else
        {
            startPos.z = 10f;
        }

        ++lineRenderer.positionCount;
        lineRenderer.SetPosition(interactNum, startPos);
        ++interactNum;
    }

    public void StopDrawing()
    {
        lineRenderer.enabled = false;
        isStart = false;
    }
}
