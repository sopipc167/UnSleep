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
    private bool onlySetting = false;

    private GameObject menuCanvas;
    private GameObject settingCanvas;
    private SettingsMenu menu;


    private void Start()
    {
        menuCanvas = transform.GetChild(0).gameObject;
        settingCanvas = transform.GetChild(1).gameObject;
        menu = GetComponent<SettingsMenu>();
    }

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
                if (PAUSE) menuCanvas.SetActive(true);
                else menuCanvas.SetActive(false);
            }
        }
    }

    public void Resume()
    {
        PAUSE = !PAUSE;
        if (PAUSE) menuCanvas.SetActive(true);
        else menuCanvas.SetActive(false);
    }

    public void GoDiary()
    {
        PAUSE = !PAUSE;
        if (PAUSE) menuCanvas.SetActive(true);
        else menuCanvas.SetActive(false);
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
        if (menuCanvas.activeSelf) menuCanvas.SetActive(false);
        else onlySetting = true;
    }

    public void OnClickSettingOff()
    {
        if (PAUSE)
        {
            isSettingOn = false;
            settingCanvas.SetActive(false);
            menuCanvas.SetActive(true);
        }
        else
        {
            if (onlySetting) isSettingOn = false;
            settingCanvas.SetActive(false);
        }
        menu.Save();
    }
}
