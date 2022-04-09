using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Build Settings 와 연계되면 관리하기 쉬울텐데....
// 에디터 커스터마이징은 끔찍해서 그냥 할게요
[SerializeField]
public enum SceneType
{
    Volcano,
    Dialogue,
    MenTal,
    Clock,
    Diary,
    Lake,
    Cave,
    Cliff,
    Title,
    Prologue,
}

public class SceneChanger
{
    public static void ChangeScene(SceneType type)
    {
        // 순서대로 세팅하긴 했는데 추후 바뀔까봐...
        //SceneManager.LoadScene((int)type);
        SceneManager.LoadScene(GetSceneName(SceneType.Volcano));
    }

    public static string GetSceneName(SceneType type)
    {
        switch (type)
        {
            case SceneType.Volcano: return "Volcano";
            case SceneType.Dialogue: return "DialogueTest";
            case SceneType.MenTal: return "Mental_World_Map";
            case SceneType.Clock: return "ClockTower";
            case SceneType.Diary: return "Diray";
            case SceneType.Lake: return "Lake";
            case SceneType.Cave: return "Cave2";
            case SceneType.Cliff: return "Cliff2";
            case SceneType.Title: return "Title";
            case SceneType.Prologue: return "Prologue";
            default: return string.Empty;
        }
    }
}
