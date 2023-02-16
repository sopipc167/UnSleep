using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveMapManager : MonoBehaviour
{
    public TextAsset caveCsv;
    public Cavern rootCavern;
    public CaveMapRenderer caveMapRenderer;

 
    private void Start()
    {
        rootCavern = new CaveMapParser().getRootCavern(caveCsv);
        caveMapRenderer.renderCavern(rootCavern);
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
        Debug.Log(string.Format("전진 {0}", routeIndex));
        // TODO
        // stack에 현재 cavern push
        // routeIndex 구멍으로 전진
        // 화면 전환 효과 재생
    }
}
