using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public enum CliffCheckType { None, Color, Shape, All, AllDiff }

public class CliffManager : MonoBehaviour
{
    public int phase;

    [Header("참조")]
    public Transform phaseGroup;
    public CliffLine line;
    private CliffRemoveManager removeManager;
    private CliffUIManager uiManager;
    private int shapeCount;

    [Header("레이어마스크 설정")]
    public LayerMask tileLayer;
    public LayerMask shapeLayer;

    [Header("삭제 가능 시 변경 색상")]
    public Color color;

    [Header("추가 옵션 설정")]
    [Tooltip("모양과 색깔이 같으면 1개로 침")]
    public bool limitedOption;
    [Tooltip("모양, 색이 모두 다른 3개를 모아도 지울 수 있음")]
    public bool additionalOption;

    private HashSet<CliffType> limitedTileSet = null;
    private class AdditionalTileSet
    {
        public HashSet<CliffShapeType> shapeSet = null;
        public HashSet<CliffColorType> colorSet = null;
    }
    private AdditionalTileSet additionalTileSet = null;

    public static bool shouldCheckStatus = false;

    internal CliffCheckType currentCheckType;
    private CliffTile currentShape;

    private bool didClick = false;
    private bool isDragging = false;
    private bool canRemove = false;

    private Camera mainCamera;
    private Vector3 mousePos;

    private void SetPhaseOption()
    {
        switch (Dialogue_Proceeder.instance.CurrentEpiID)
        {
            case 3:
                limitedOption = false;
                additionalOption = false;
                phase = 1;
                break;
            case 5:
                limitedOption = false;
                additionalOption = false;
                phase = 2;
                break;
            case 9:
                limitedOption = true;
                additionalOption = false;
                phase = 3;
                break;
            case 14:
                limitedOption = true;
                additionalOption = false;
                phase = 4;
                break;
            case 17:
                limitedOption = true;
                additionalOption = true;
                phase = 5;
                break;
            default:
                limitedOption = true;
                additionalOption = true;
                phase = 6;
                break;
        }
    }

    void Start()
    {
        mainCamera = Camera.main;
        removeManager = GetComponent<CliffRemoveManager>();
        uiManager = GetComponent<CliffUIManager>();

        SetPhaseOption();
        int size = phaseGroup.childCount;
        for (int i = 0; i < size; i++)
        {
            if (i == phase - 1)
            {
                GameObject currentPhase = phaseGroup.GetChild(i).gameObject;
                currentPhase.SetActive(true);
                shapeCount = currentPhase.transform.GetChild(1).childCount;
            }
            else
            {
                phaseGroup.GetChild(i).gameObject.SetActive(false);
            }
        }

        if (limitedOption)
        {
            limitedTileSet = new HashSet<CliffType>();
        }
        if (additionalOption)
        {
            additionalTileSet = new AdditionalTileSet();
            additionalTileSet.shapeSet = new HashSet<CliffShapeType>();
            additionalTileSet.colorSet = new HashSet<CliffColorType>();
        }
    }

    private void FixedUpdate()
    {
        if (didClick)
        {
            didClick = false;
            shouldCheckStatus = false;
            currentShape = GetCurrentShape();

            //Not a tile
            if (currentShape == null)
            {
                ResetData();
                return;
            }

            //if (currentShape.isReverting || currentShape.isDestroying)
            //{
            //    Debug.Log("현재 타일은 상호작용중이야!");
            //    ResetData();
            //    return;
            //}

            //Revert Tile
            if (currentShape.canRevert)
            {
                int cnt = removeManager.RevertTiles(currentShape);
                uiManager.ChangeProgress(-cnt * 100f / shapeCount);
                ResetData();
                return;
            }

            //Start Interaction
            if (currentShape.type.shape != CliffShapeType.None && currentShape.type.color != CliffColorType.None)
            {
                currentCheckType = CliffCheckType.All;
                AddRemoveTile();
                line.DrawLine(currentShape.transform);
                uiManager.ChangeInteractNumText(1);
                uiManager.ChangeTypeText(currentCheckType, currentShape);
            }
            else
            {
                Debug.Log("이상한 곳 누르지마!!");
                ResetData();
            }
        }
        else if (isDragging)
        {
            if (shouldCheckStatus)
            {
                shouldCheckStatus = false;
                mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                CliffTile tmp = GetCurrentShape();
                if (tmp == null) return;

                Vector3 offset = currentShape.transform.position - tmp.transform.position;
                if (offset.sqrMagnitude > 4.90) return;
                if (removeManager.IsContainTile(tmp)) return;

                CliffCheckType check = GetCheckType(tmp);

                if (check != CliffCheckType.None)
                {
                    currentShape = tmp;
                    currentCheckType = check;
                    uiManager.ChangeTypeText(currentCheckType, currentShape);
                    AddRemoveTile();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!ExceptUIClick.isActive && !MemoManager.isMemoOn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                didClick = true;
                isDragging = true;
                mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (canRemove)
                {
                    uiManager.ChangeProgress(removeManager.GetTileCount * 100f / shapeCount);
                    removeManager.RemoveTiles();

                    if (uiManager.GetProgress == 100)
                    {
                        removeManager.ClearPhase();
                    }
                }
                ResetData();
            }

            if (Input.GetMouseButtonDown(1))
            {
                ResetData();
            }
        }
    }

    private CliffCheckType GetCheckType(CliffTile tmpShape)
    {
        if (currentCheckType == CliffCheckType.AllDiff)
        {
            if (limitedOption)
            {
                if (removeManager.FindTile(tmpShape))
                {
                    return CliffCheckType.AllDiff;
                }
            }

            if (additionalTileSet.colorSet.Contains(tmpShape.type.color) ||
                additionalTileSet.shapeSet.Contains(tmpShape.type.shape))
            {
                return CliffCheckType.None;
            }
            else
            {
                return CliffCheckType.AllDiff;
            }
        }

        CliffCheckType check = GetIncidentCheck(tmpShape);

        if (check == CliffCheckType.All)
        {
            check = currentCheckType;
        }
        else if (check != currentCheckType && currentCheckType != CliffCheckType.All)
        {
            check = CliffCheckType.None;
        }

        if (additionalOption)
        {
            if (check == CliffCheckType.None && removeManager.GetInteractNum == 1)
            {
                return CliffCheckType.AllDiff;
            }
        }

        return check;
    }

    private CliffTile GetCurrentShape()
    {
        Collider2D target = Physics2D.OverlapPoint(mousePos, shapeLayer);
        if (target != null)
        {
            if (currentShape != null)
            {
                if (currentShape == target)
                {
                    return currentShape;
                }
            }
            return target.GetComponent<CliffTile>();
        }
        else
        {
            return null;
        }
    }

    private CliffCheckType GetIncidentCheck(CliffTile tmpShape)
    {
        CliffCheckType result = CliffCheckType.None;

        if (currentShape.type.shape == tmpShape.type.shape)
        {
            result = CliffCheckType.Shape;
        }

        if (currentShape.type.color == tmpShape.type.color)
        {
            if (result == CliffCheckType.Shape)
            {
                result = CliffCheckType.All;
            }
            else
            {
                result = CliffCheckType.Color;
            }
        }
        return result;
    }

    private void AddRemoveTile()
    {
        bool isIncreaseIntreraction = true;
        if (limitedOption)
        {
            if (limitedTileSet.Contains(currentShape.type))
            {
                isIncreaseIntreraction = false;
            }
            else
            {
                limitedTileSet.Add(currentShape.type);
            }
        }

        if (removeManager.AddTile(currentShape, isIncreaseIntreraction))
        {
            uiManager.ChangeInteractNumText(removeManager.GetInteractNum);
            if (additionalOption)
            {
                additionalTileSet.shapeSet.Add(currentShape.type.shape);
                additionalTileSet.colorSet.Add(currentShape.type.color);
            }

            line.SetPoint(currentShape.transform);

            if (!canRemove && isIncreaseIntreraction && removeManager.CanRemove())
            {
                canRemove = true;
                removeManager.ChangeColor(color);
            }
            else if (canRemove)
            {
                removeManager.ChangeColor(color);
            }
        }
    }

    private void ResetData()
    {
        isDragging = false;
        currentShape = null;
        canRemove = false;
        line.StopDrawing();
        removeManager.ResetTile();
        uiManager.ChangeTypeText(CliffCheckType.None);
        uiManager.ChangeInteractNumText(0);

        if (limitedOption)
        {
            limitedTileSet.Clear();
        }
        if (additionalOption)
        {
            additionalTileSet.shapeSet.Clear();
            additionalTileSet.colorSet.Clear();
        }
    }
}