using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_CliffClear : MonoBehaviour
{
    private CliffTile[] tiles;
    private GameObject flagObj;

    private void Awake()
    {
        int i, size = transform.childCount;

        tiles = new CliffTile[size];
        for (i = 0; i < size; ++i)
        {
            tiles[i] = transform.GetChild(i).GetChild(1).GetComponent<CliffTile>();
        }
        flagObj = tiles[0].gameObject;
    }

    void Start()
    {
        foreach (var item in tiles)
        {
            item.transform.rotation = Quaternion.Euler(Vector3.zero);   //이상하게 이거 빼면 x=90 됨..
            item.ClearPhase();
        }
    }

    void Update()
    {
        if (flagObj.activeSelf) return;

        foreach (var item in tiles)
        {
            item.transform.localScale = Vector3.one;
            item.gameObject.SetActive(true);
            item.ClearPhase();
        }
    }
}
