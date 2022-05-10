using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EggBab9 : MonoBehaviour
{
    TextManager textManager;

    // Start is called before the first frame update
    void Start()
    {
        textManager = GameObject.Find("Manager").GetComponent<TextManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (textManager.Dia_index == 911 && textManager.dialogues_index == 18)
            Disappear();
        //911-18
    }

    public void Disappear()
    {
        GetComponent<Image>().DOFade(0f, 1f);
    }
}
