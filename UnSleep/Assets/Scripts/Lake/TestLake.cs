using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLake : MonoBehaviour
{
    public static bool isOpen = true;
    public GameObject clear;

    private void Awake()
    {
        if (!isOpen)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        //if (LakeManager.currentPhase == 5)
        //{
        //    LakeManager.currentPhase = 1;
        //    clear.SetActive(true);
        //}
    }
}
