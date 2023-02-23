using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveMapManager : MonoBehaviour, DialogueDoneListener
{
    public TextAsset caveCsv;
    public GameObject backButton;
    public CaveMapRenderer caveMapRenderer;
    public TextManager textManager;
    public PuzzleClear puzzleClear;
    public GameObject DiaUI;

    private Stack<Cavern> stack = new Stack<Cavern>();
    private Cavern rootCavern;
    private Cavern currentCavern;

    public bool DiaActive
    {
        get
        {
            return DiaUI.activeSelf;
        }
    }

    private void Start()
    {
        rootCavern = new CaveMapParser().getRootCavern(caveCsv);
        currentCavern = rootCavern;
        caveMapRenderer.renderCavern(currentCavern);
        textManager.addDialogueDoneListeners(this);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero, 0f);

            if (hit.collider != null)
            {
                Hole hole = hit.collider.gameObject.GetComponent<Hole>();
                if (hole != null)
                {
                    (hole as CaveClickable).onClick(this);
                }
            }
        }
    }

    public void proceed(int routeIndex)
    {
        if (DiaActive || caveMapRenderer.moving) return;

        stack.Push(currentCavern);
        currentCavern = currentCavern.next[routeIndex];
        
        caveMapRenderer.proceed(currentCavern);
        if (!backButton.activeSelf) backButton.SetActive(true);

        if (currentCavern.routeCnt == 999)
        {
            backButton.SetActive(false);
        }
        
    }

    public void back()
    {
        if (DiaActive || caveMapRenderer.moving) return;

        currentCavern = stack.Pop();
        caveMapRenderer.back(currentCavern);
        if (stack.Count == 0) backButton.SetActive(false);
       
    }

    public void OnDialogueEnd(int DiaId)
    {
        Debug.Log(string.Format("OnDialogueEnd : {0}", DiaId));
        if (currentCavern.routeCnt == 999 && currentCavern.talkId == DiaId)
        {
            Debug.Log("끝~");
            puzzleClear.gameObject.SetActive(true);
            //PuzzleClear puzzleClear = Clear.transform.GetChild(0).GetComponent<PuzzleClear>();
            SoundManager.Instance.FadeOutBGM();
            int CurEpiId = Dialogue_Proceeder.instance.CurrentEpiID;
            int CurDiaId = Dialogue_Proceeder.instance.CurrentDiaID;


            if (CurEpiId == 7)
            {
                if (CurDiaId == 2013)
                    puzzleClear.ClearPuzzle(SceneType.Mental, 1f);
                else if (CurDiaId == 2017)
                    puzzleClear.ClearPuzzle(SceneType.Dialogue, 1f);
            }
            else if (CurEpiId == 9 || CurEpiId == 11 || CurEpiId == 15 || CurEpiId == 16 || CurEpiId == 18 || CurEpiId == 19) //나중엔 퍼즐 연출로
                puzzleClear.ClearPuzzle(SceneType.Mental, 1f);
            else if (CurEpiId == 2 || CurEpiId == 5 || CurEpiId == 17)
                puzzleClear.ClearPuzzle(SceneType.Dialogue, 1f);

        }
    }
}
