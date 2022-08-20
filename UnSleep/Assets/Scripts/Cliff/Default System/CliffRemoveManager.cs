using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CliffRemoveManager : MonoBehaviour
{
    [Header("참조")]
    public LineRenderer lineEffect;
    public LayerMask shapeLayer;
    public PuzzleClear clearCanvas;
    public TextManager textManager;

    //전체 타일 저장
    public HashSet<CliffTile> cliffTiles = new HashSet<CliffTile>();

    //Revert할 타일들의 묶음
    private Dictionary<CliffTile, LineRenderer> tileDic = new Dictionary<CliffTile, LineRenderer>();

    //현재 상호작용중인 타일 묶음
    private List<CliffTile> tileList = new List<CliffTile>();

    private int interactNum;
    private bool changeFlag = false;
    public int GetInteractNum { get { return interactNum; } }
    public int GetTileCount { get { return tileList.Count; } }

    public bool CanRemove()
    {
        if (interactNum >= 3) return true;
        else return false;
    }

    public bool IsContainTile(CliffTile tile)
    {
        return tileList.Contains(tile);
    }

    public bool FindTile(CliffTile tile)
    {
        int cnt = tileList.Count;
        for (int i = 0; i < cnt; i++)
        {
            if (tileList[i].type.color == tile.type.color &&
                tileList[i].type.shape == tile.type.shape)
            {
                return true;
            }
        }
        return false;
    }

    //Add가 성공적으로 됐다면 true, 아니라면 false
    public bool AddTile(CliffTile currentTile, bool isIncreaseIntreraction = true)
    {
        cliffTiles.Add(currentTile);
        tileList.Add(currentTile);
        if (isIncreaseIntreraction)
            ++interactNum;

        return true;
    }

    public void RemoveTiles()
    {
        int cnt = tileList.Count;

        LineRenderer lineRenderer = tileList[0].GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = tileList[0].gameObject.AddComponent<LineRenderer>();
            lineRenderer.colorGradient = lineEffect.colorGradient;
            lineRenderer.sortingLayerID = lineEffect.sortingLayerID;
            lineRenderer.sortingOrder = lineEffect.sortingOrder;
            lineRenderer.material = lineEffect.material;
            lineRenderer.alignment = lineEffect.alignment;
            lineRenderer.widthCurve = lineEffect.widthCurve;
            lineRenderer.numCapVertices = lineEffect.numCapVertices;
        }
        lineRenderer.positionCount = cnt;
        lineRenderer.enabled = true;

        for (int i = 0; i < cnt; i++)
        {
            tileList[i].parentTile = tileList[0];

            Vector3 pos = tileList[i].transform.position;

            //라인렌데러를 조금 더 부드럽게 만들어줌
            if (i % 2 != 0) pos.z = 40f;
            else pos.z = 10f;

            lineRenderer.SetPosition(i, pos);
            tileList[i].DestroyShape();
        }

        tileDic[tileList[0]] = lineRenderer;
        interactNum = 0;
        tileList.Clear();
    }

    public int RevertTiles(CliffTile currentTile)
    {
        LineRenderer lineRenderer = tileDic[currentTile.parentTile];

        int cnt = lineRenderer.positionCount;
        for (int i = 0; i < cnt; i++)
        {
            Collider2D target = Physics2D.OverlapPoint(lineRenderer.GetPosition(i), shapeLayer);
            if (target != null)
            {
                target.GetComponent<CliffTile>().RevertShape();
            }
            else
            {
                Debug.Log("Revert 오류");
                continue;
            }
        }

        tileDic.Remove(currentTile);
        lineRenderer.enabled = false;
        return cnt;
    }

    public void ChangeColor(Color color)
    {
        if (!changeFlag)
        {
            changeFlag = true;
            for (int i = 0; i < tileList.Count; i++)
            {
                tileList[i].ChangeColor(color);
            }
        }
        else
        {
            tileList[tileList.Count - 1].ChangeColor(color);
        }
    }

    public void ResetTile()
    {
        interactNum = 0;
        changeFlag = false;
        ChangeColor(Color.white);
        tileList.Clear();
    }

    public void ClearPhase()
    {
        foreach (var i in tileDic)
        {
            i.Value.positionCount = 0;
        }
        tileDic.Clear();

        foreach (var i in cliffTiles)
        {
            i.ClearPhase();
        }
        cliffTiles.Clear();

        if (Dialogue_Proceeder.instance.CurrentEpiID == 19) //잘 있어요 용 조건 추가. 일단 미개하게 집어넣고 추후 수정이 필요하겠습니다
        {
            Dialogue_Proceeder.instance.AddCompleteCondition(61);
            textManager.Set_Dialogue_Goodbye();
            return;
        }
            
        //conflict merge할때 여기 실수로 제꺼랑 스까해서 덮었네요 원상복구함
        clearCanvas.ClearPuzzle(SceneType.Mental, 1f);
    }

    
}
