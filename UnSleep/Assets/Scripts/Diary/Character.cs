using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public GameObject Ex;
    public Image img;


    public void Enter()
    {
        Ex.SetActive(true);
        img.color = new Color32(255, 255, 255, 255);
    }

    public void Exit()
    {
        img.color = new Color32(156, 156, 156, 255);
        Ex.SetActive(false);
    }
}
