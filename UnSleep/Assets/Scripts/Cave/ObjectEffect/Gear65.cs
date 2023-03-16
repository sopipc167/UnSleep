using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gear65 : MonoBehaviour
{

    public GameObject gearBtn;
    private ObjectManager objectManager;
    public CaveStopPanel caveStopPanel;
    private CaveMapManager caveMapManager;

    private void Start()
    {
        caveMapManager = GameObject.Find("CaveSystem").GetComponent<CaveMapManager>();
        objectManager = GameObject.Find("CaveSystem").GetComponent<ObjectManager>();
    }



    public void ClickGearBtn()
    {
        gearBtn.SetActive(false);
        objectManager.OnGearUI();
        caveMapManager.objectActive = false;
        caveStopPanel.disableCaveStopPanel();
    }
}
