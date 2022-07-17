using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    private static MenuManager instance = null;
    private bool PAUSE = false;

    public GameObject MenuCanvas;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PAUSE = !PAUSE;
        }

        if (PAUSE)
        {
            MenuCanvas.SetActive(true);
        }
        else
        {
            MenuCanvas.SetActive(false);
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
}
