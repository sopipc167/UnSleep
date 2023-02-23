using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveMapManager : MonoBehaviour, DialogueDoneListener
{
    public TextAsset caveCsv;
    public GameObject backButton;

    public Cavern rootCavern;
    private Cavern currentCavern;
    public CaveMapRenderer caveMapRenderer;
    public TextManager textManager;

    private Stack<Cavern> stack = new Stack<Cavern>();

    public GameObject DiaUI;
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
        }
    }
}
