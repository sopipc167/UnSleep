using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundCompassMaster : MonoBehaviour
{
    public GameObject compass;

    private void Start()
    {
        GetComponent<Toggle>().onValueChanged.AddListener(delegate { compassSetActive(); });
    }

    public void compassSetActive()
    {
        compass.SetActive(!compass.activeSelf);
        // Debug.Log(compass.activeSelf);
        // if (compass.activeSelf) compass.SetActive(false); else compass.SetActive(true);
    }
    
}
