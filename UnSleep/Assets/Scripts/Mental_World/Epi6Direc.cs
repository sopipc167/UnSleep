using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Epi6Direc : MonoBehaviour
{
    private GameObject mwpuzzle;


    void Start()
    {
        mwpuzzle = GameObject.Find("Cinematic").transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Dialogue_Proceeder.instance.CurrentDiaID == 604 && !mwpuzzle.activeSelf)
        {
            mwpuzzle.SetActive(true);
            Debug.Log("엥?");
        }
            
    }
}
