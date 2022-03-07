using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CliffLine의 LineRenderer는 1개이기 때문에 여러 곳에서 참조하면 오류가 일어남
// 따라서 부모 클래스로 만들어 기능을 그대로 가져온다.

public class ShowCliffDestroy : CliffLine
{
    private CliffTile[] tiles;
    private int tileCount;
    private bool repeatFlag = false;
    private readonly WaitForSeconds destroyDelay = new WaitForSeconds(2f);


    // Start is called before the first frame update
    protected override void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.numCornerVertices = 5;

        tileCount = transform.childCount;
        tiles = new CliffTile[tileCount];
        int i = 0;
        foreach (Transform item in transform)
        {
            tiles[i] = item.GetChild(1).GetComponent<CliffTile>();
            tiles[i].startAniFlag = false;
            ++i;
        }
    }

    protected override void Update()
    {
        if (repeatFlag) return;

        repeatFlag = true;
        StartCoroutine(RepeatDrawingCoroutine());
    }

    private void OnDisable()
    {
        for (int i = 0; i < tileCount; ++i)
        {
            tiles[i].ChangeAlpha(1f);
        }
        StopDrawing();
        repeatFlag = false;
    }

    // 그리기 시작할 때 호출
    public override void DrawLine(Transform target)
    {
        targetPos = target;
        startPos = target.position;
        startPos.z = 10f;

        lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPos);
        interactNum = 1;
    }

    // 그리고 있는 점을 추가할 때 호출: SetPoint(Transform target);


    // 차례대로 그리기 시작
    private IEnumerator RepeatDrawingCoroutine()
    {
        // 첫번째 자식을 시작으로 그리기 시작, startPos에 값 할당
        DrawLine(tiles[0].transform);
        lineRenderer.SetPosition(interactNum, startPos);

        // 모든 타일을 차례대로 그림
        for (int i = 1; i < tileCount; ++i)
        {
            endPos = startPos;
            Transform curTarget = tiles[i].transform;
            Vector3 diff = curTarget.position - startPos;
            endPos.z = 0f;
            diff.z = 0f;
            curTarget.position = new Vector3(curTarget.position.x, curTarget.position.y, 0f);

            while ((endPos - curTarget.position).magnitude > 0.1f)
            {
                // 1초만에 선이 1개 그어짐
                endPos += diff * Time.deltaTime;
                lineRenderer.SetPosition(interactNum, endPos);
                yield return null;
            }
            // 점 고정, startPos 갱신
            SetPoint(curTarget);
        }
        --lineRenderer.positionCount;
        RemoveTiles();

        yield return destroyDelay;
        for (int i = 0; i < tileCount; ++i)
        {
            tiles[i].ChangeAlpha(1f);
        }
        StopDrawing();
        repeatFlag = false;
    }

    private void RemoveTiles()
    {
        lineRenderer.positionCount = tileCount;
        lineRenderer.enabled = true;

        for (int i = 0; i < tileCount; i++)
        {
            Vector3 pos = tiles[i].transform.position;

            //라인렌데러를 조금 더 부드럽게 만들어줌
            if (i % 2 != 0) pos.z = 40f;
            else pos.z = 10f;

            lineRenderer.SetPosition(i, pos);
            tiles[i].DestroyShape();
        }

        interactNum = 0;
    }
}
