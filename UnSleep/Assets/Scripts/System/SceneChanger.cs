using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Build Settings 와 연계되면 관리하기 쉬울텐데....
// 에디터 커스터마이징은 끔찍해서 그냥 할게요
[SerializeField]
public enum SceneType
{
    None = -1,
    Title,
    Prologue,
    Diary,
    Dialogue,
    Mental,
    Lake,
    Volcano,
    ClockTower,
    Nightmare,
    Nightmare27,
    Cave,
    Cliff,
}

public class SceneChanger
{
    public static void ChangeScene(SceneType type)
    {
        // 순서대로 세팅하긴 했는데 추후 바뀔까봐...
        //SceneManager.LoadScene((int)type);
        SceneManager.LoadScene(GetSceneName(type));
    }

    public static string GetSceneName(SceneType type)
    {
        switch (type)
        {
            case SceneType.Volcano: return "Volcano";
            case SceneType.Dialogue: return "DialogueTest";
            case SceneType.Mental: return "Mental_World_Map";
            case SceneType.ClockTower: return "ClockTower";
            case SceneType.Diary: return "Diary";
            case SceneType.Lake: return "Lake";
            case SceneType.Cave: return "Cave";
            case SceneType.Cliff: return "Cliff";
            case SceneType.Title: return "Title";
            case SceneType.Prologue: return "Prologue";
            case SceneType.Nightmare: return "Nightmare";
            case SceneType.Nightmare27: return "Nightmare_27";
            default: return string.Empty;
        }
    }

    public static void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
