using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveMapRenderer : MonoBehaviour
{
    public SpriteRenderer background;
    public Sprite[] backgroundSprites;

    public GameObject[] holes;


    public void renderCavern(Cavern cavern)
    {
        if (cavern.routeCnt < 0) return; // 오류 방지 early return

        background.sprite = backgroundSprites[cavern.routeCnt];
        
        foreach(GameObject h in holes)
        {
            h.SetActive(false);
        }

        holes[cavern.routeCnt].SetActive(true);
    }
}
