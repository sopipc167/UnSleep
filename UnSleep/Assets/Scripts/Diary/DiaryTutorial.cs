using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryTutorial : MonoBehaviour
{
    

    void Start()
    {
        if (SaveDataManager.Instance.Progress == 0)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
