using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DiaryDataTester : MonoBehaviour
{
    public void clear()
    {
        int ClearData = SaveDataManager.Instance.LoadEpiProgress();
        ClearData = ClearData + 1 > 20 ? 20 : ClearData + 1;
        SaveDataManager.Instance.SaveEpiProgress(ClearData);
        reload();
    }

    public void reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void revertClear()
    {
        int ClearData = SaveDataManager.Instance.LoadEpiProgress();

        ClearData = ClearData - 1 < 0 ? 0 : ClearData - 1;
        //SaveDataManager.Instance.Progress = ClearData;
        SaveDataManager.Instance.SaveEpiProgress(ClearData);
        reload();
    }

    public void reset()
    {
        SaveDataManager.Instance.SaveEpiProgress(0);
        reload();
    }
}
