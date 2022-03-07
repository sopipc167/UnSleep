using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCliffClear : MonoBehaviour
{
    private CliffTile[] tiles;
    private GameObject flagObj;


    private void Awake()
    {
        tiles = new CliffTile[transform.childCount];
        int i = 0;
        foreach (Transform item in transform)
        {
            tiles[i] = item.GetChild(1).GetComponent<CliffTile>();
            tiles[i].startAniFlag = false;
            tiles[i].gameObject.SetActive(false);
            ++i;
        }
        flagObj = tiles[0].gameObject;
    }

    private void Update()
    {
        if (flagObj.activeSelf) return;

        foreach (var item in tiles)
        {
            item.transform.localScale = Vector3.one;
            item.gameObject.SetActive(true);
            item.ClearPhase();
        }
    }

    private void OnDisable()
    {
        foreach (var item in tiles)
        {
            item.gameObject.SetActive(false);
        }
    }
}
