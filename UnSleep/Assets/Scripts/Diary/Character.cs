using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Character : MonoBehaviour
{
    public Image Ex;

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void Enter()
    {
        Ex.gameObject.SetActive(true);
    }

    public void Exit()
    {
        Ex.gameObject.SetActive(false);
    }
}
