using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    #region 싱글톤 클래스
    private static MenuManager instance;

    public static MenuManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<MenuManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    MenuManager newObj = Resources.Load<MenuManager>("Singleton/MenuManager");
                    instance = Instantiate(newObj);
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    private bool PAUSE = false;
    private bool isSettingOn = false;

    public GameObject MenuCanvas;
    public GameObject settingCanvas;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isSettingOn)
            {
                OnClickSettingOff();
            }
            else
            {
                PAUSE = !PAUSE;
                if (PAUSE) MenuCanvas.SetActive(true);
                else MenuCanvas.SetActive(false);
            }
        }
    }

    public void Resume()
    {
        PAUSE = !PAUSE;
    }

    public void GoDiary()
    {
        PAUSE = !PAUSE;
        SceneManager.LoadScene("Diary");
    }

    public void Exit_()
    {
        Application.Quit();
    }

    public void OnClickSettingOn()
    {
        isSettingOn = true;
        settingCanvas.SetActive(true);
        MenuCanvas.SetActive(false);
    }
    public void OnClickSettingOff()
    {
        isSettingOn = false;
        settingCanvas.SetActive(false);
        MenuCanvas.SetActive(true);
    }
}
