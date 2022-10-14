using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public GameObject continueBtn;
    public AudioClip titleBGM;


    private IEnumerator Start()
    {
        SaveDataManager.Instance.LoadEpiProgress(); //세이브 파일 가져오기

        if (SaveDataManager.Instance.Progress == 0) //세이브 파일 없으면  
        {
            continueBtn.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
            continueBtn.GetComponent<Button>().enabled = false;
        }

        yield return new WaitForSeconds(1f);
        Debug.Log("시작");
        SoundManager.Instance.PlayBGM(titleBGM);
    }


    public void ContinueGame()
    {
        SceneChanger.ChangeScene(SceneType.Diary);
    }

    public void NewGame()
    {
        SaveDataManager.Instance.SaveEpiProgress(0);
        SceneManager.LoadScene("Prologue");
    }

    public void SettingGame()
    {
        //설정 창 오픈
    }
}
