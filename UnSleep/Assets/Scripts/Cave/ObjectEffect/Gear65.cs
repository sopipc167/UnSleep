using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gear65 : MonoBehaviour
{

    public GameObject gearBtn;
    private ObjectManager objectManager;

    private void Start()
    {
        objectManager = GameObject.Find("MapManager").GetComponent<ObjectManager>();
    }



    public void ClickGearBtn()
    {
        gearBtn.SetActive(false);
        objectManager.OnGearUI();
    }
}
