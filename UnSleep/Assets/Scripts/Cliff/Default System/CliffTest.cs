using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CliffTest : MonoBehaviour
{
    [Header("참조")]
    public GameObject mainCanvas;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mainCanvas.SetActive(!mainCanvas.activeSelf);
        }
    }

    public void OnClickButton(int phase)
    {
        SceneChanger.RestartScene();
    }

    public void OnClickExitButton()
    {
        Application.Quit();
    }
}
