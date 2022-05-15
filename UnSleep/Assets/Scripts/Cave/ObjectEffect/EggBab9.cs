using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EggBab9 : MonoBehaviour
{
    Dialogue_Proceeder dp;

    private void Start()
    {
        dp = Dialogue_Proceeder.instance;
    }

    void Update()
    {
        if (dp.CurrentDiaID == 911 && dp.CurrentDiaIndex == 18)
            Disappear();
        //911-18
    }

    public void Disappear()
    {
        GetComponent<Image>().DOFade(0f, 1f);
    }
}
