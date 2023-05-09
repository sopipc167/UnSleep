using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnabledCube : MonoBehaviour
{
    public GameObject MWPuzzle;
    private bool flag = false;

    // Update is called once per frame
    void Update()
    {
        if (flag) return;
        if (!flag && MWPuzzle.activeSelf)
        {
            flag = true;
            StartCoroutine(ActiveCoroutine()) ;
        }
    }

    private IEnumerator ActiveCoroutine()
    {
        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(false);
        }
        yield return new WaitUntil(() => !MWPuzzle.activeSelf);
        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(true);
        }
    }
}
