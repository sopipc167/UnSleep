using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [SerializeField] private Button tutorialButton;
    public AudioClip clip;
    private TutorialHelper tutorial;

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
        if (Input.GetKeyDown(KeyCode.Escape) && SceneChanger.Instance.IsDone)
        {
            if (MemoManager.isMemoOn) return;
            if (isSettingOn)
            {
                OnClickSettingOff();
            }
            else
            {
                GameManager.IsPause = !GameManager.IsPause;
                if (GameManager.IsPause) ShowMenu();
                else menuCanvas.SetActive(false);
            }
        }
    }

    private void ShowMenu()
    {
        tutorialButton.interactable = SceneChanger.Instance.IsPuzleScene();
        menuCanvas.SetActive(true);
    }

    public void Resume()
    {
        SoundManager.Instance.PlaySE(clip);
        GameManager.IsPause = false;
        if (GameManager.IsPause) ShowMenu();
        else menuCanvas.SetActive(false);
    }

    public void GoDiary()
    {
        SoundManager.Instance.PlaySE(clip);
        GameManager.IsPause = false;
        if (GameManager.IsPause) ShowMenu();
        else menuCanvas.SetActive(false);
        SoundManager.Instance.StopSE();
        SceneChanger.Instance.ChangeScene(SceneType.Diary);
    }

    public void Exit_()
    {
        SoundManager.Instance.PlaySE(clip);
        Application.Quit();
    }

    public void OnClickSettingOn()
    {
        SoundManager.Instance.PlaySE(clip);
        isSettingOn = true;
        settingCanvas.SetActive(true);
        if (menuCanvas.activeSelf) menuCanvas.SetActive(false);
        else onlySetting = true;
    }

    public void OnClickSettingOff()
    {
        SoundManager.Instance.PlaySE(clip);

        if (GameManager.IsPause)
        {
            isSettingOn = false;
            settingCanvas.SetActive(false);
            ShowMenu();
        }
        else
        {
            if (onlySetting) isSettingOn = false;
            settingCanvas.SetActive(false);
        }
        menu.Save();
    }

    public void OnClickTutorialOn()
    {
        SoundManager.Instance.PlaySE(clip);

        GameManager.IsPause = false;
        if (tutorial == null)
        {
            tutorial = GameObject.FindGameObjectWithTag("Tutorial").transform.GetChild(0).GetComponent<TutorialHelper>();
        }
        tutorial.HowToPuzzle();
        menuCanvas.SetActive(false);
    }
}
