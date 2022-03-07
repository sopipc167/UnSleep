using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCliffRevert : MonoBehaviour
{
    private CliffTile[] tiles;
    private LineRenderer lineRenderer;
    private bool flag = false;

    private WaitForSeconds delay1 = new WaitForSeconds(1.2f);
    private WaitForSeconds delay2 = new WaitForSeconds(0.5f);

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        tiles = new CliffTile[transform.childCount];
        Vector3 tmp;
        int i = 0;
        foreach (Transform item in transform)
        {
            tiles[i] = item.GetChild(1).GetComponent<CliffTile>();
            tiles[i].startAniFlag = false;
            tmp = item.position;
            if (i % 2 != 0) tmp.z = 40f;
            else tmp.z = 10f;
            lineRenderer.SetPosition(i, tmp);
            ++i;
        }
    }

    private void Start()
    {
        flag = true;
        StartCoroutine(RevertCoroutine());
    }

    private void OnEnable()
    {
        if (flag)
        {
            StartCoroutine(RevertCoroutine());
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Init()
    {
        foreach (var item in tiles)
        {
            item.ChangeAlpha(CliffTile.ALPHA_VALUE);
        }
        lineRenderer.enabled = true;
    }

    private IEnumerator RevertCoroutine()
    {
        while (true)
        {
            Init();
            yield return delay2;

            lineRenderer.enabled = false;
            foreach (var item in tiles)
            {
                item.RevertShape();
            }
            yield return delay1;
        }
    }
}
