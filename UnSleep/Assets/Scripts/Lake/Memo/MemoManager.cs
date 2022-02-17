using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DrawLine))]
[RequireComponent(typeof(EraseLine))]

public class MemoManager : MonoBehaviour
{
    [Header("기본 참조")]
    public GameObject backgroundCanvas;
    public Button mainButton;
    public GameObject menuCanvas;
    public GameObject memoDetailCanvas;
    public GameObject eraseDetailCanvas;

    [Header("버튼 참조")]
    public Button memoButton;
    public Button eraseButton;
    public Button deleteAllButton;

    [Header("카메라 참조")]
    public Camera memoCamera;

    public static bool isMemoOn = false;
    internal bool isEraseMode = false;  //Erase(true) or Draw(false)

    private void Awake()
    {
        mainButton.onClick.AddListener(OnClickMainButton);
        memoButton.onClick.AddListener(OnClickMemoButton);
        eraseButton.onClick.AddListener(OnClickEraseButton);
    }

    public void OnClickActiveToggle(GameObject me)
    {
        me.SetActive(!me.activeSelf);
    }

    private void OnClickMainButton()
    {
        isMemoOn = !isMemoOn;
        if (isMemoOn)
        {
            backgroundCanvas.SetActive(true);
            menuCanvas.SetActive(true);
            isEraseMode = false;
        }
        else
        {
            backgroundCanvas.SetActive(false);
            menuCanvas.SetActive(false);
            memoDetailCanvas.SetActive(false);
            eraseDetailCanvas.SetActive(false);
        }
    }

    private void OnClickMemoButton()
    {
        if (!isEraseMode)
        {
            memoDetailCanvas.SetActive(!memoDetailCanvas.activeSelf);
        }
        else
        {
            eraseDetailCanvas.SetActive(false);
            isEraseMode = false;
        }
    }

    private void OnClickEraseButton()
    {
        if (isEraseMode)
        {
            eraseDetailCanvas.SetActive(!eraseDetailCanvas.activeSelf);
        }
        else
        {
            memoDetailCanvas.SetActive(false);
            isEraseMode = true;
        }
    }
}
