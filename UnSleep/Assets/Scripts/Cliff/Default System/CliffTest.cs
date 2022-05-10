using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClickExitButton()
    {
        Application.Quit();
    }
}
