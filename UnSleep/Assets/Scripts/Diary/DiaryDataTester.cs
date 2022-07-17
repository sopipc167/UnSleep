using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DiaryDataTester : MonoBehaviour
{

    public Button btn;
    public Button rebtn;


    public void pushBtn()
    {
        int ClearData = SaveDataManager.Instance.LoadEpiProgress();
        SaveDataManager.Instance.SaveEpiProgress(++ClearData);
    }

    public void reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
