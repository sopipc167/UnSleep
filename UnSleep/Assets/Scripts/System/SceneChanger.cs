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

public class SceneChanger : MonoBehaviour
{
    #region 싱글톤 클래스
    private static SceneChanger instance;

    public static SceneChanger Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<SceneChanger>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    SceneChanger newObj = Resources.Load<SceneChanger>("Singleton/SceneChanger");
                    instance = Instantiate(newObj);
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        transition = GetComponent<SceneTransition>();
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    private SceneTransition transition;

    private readonly WaitForSeconds delay = new WaitForSeconds(0.1f);
    private WaitUntil untilTransition;

    public SceneTransition Transition { get => transition; }
    public bool IsDone { get; private set; } = false;

    private void Start()
    {

        untilTransition = new WaitUntil(() => transition.IsDone);
    }

    public void ChangeScene(SceneType type, bool isFade = true, float fadeTime = 1f)
    {
        if (isFade)
        {
            IsDone = false;
            StartCoroutine(ChangeSceneCoroutine(GetSceneName(type), fadeTime));
        }
        else
        {
            SceneManager.LoadScene(GetSceneName(type));
        }
        MemoManager.isMemoOn = false;
    }

    public void RestartScene(bool isFade = true, float fadeTime = 1.5f)
    {
        if (isFade)
        {
            IsDone = false;
            StartCoroutine(ChangeSceneCoroutine(SceneManager.GetActiveScene().name, fadeTime));
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public string GetSceneName(SceneType type)
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

    public bool IsPuzleScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log(sceneName);
        if (sceneName.Equals("Volcano") ||
            sceneName.Equals("ClockTower") ||
            sceneName.Equals("Lake") ||
            sceneName.Equals("Cave") ||
            sceneName.Equals("Cliff")) return true;
        return false;
    }

    private IEnumerator ChangeSceneCoroutine(string type, float fadeTime)
    {
        transition.FadeOut(fadeTime);

        // 비동기 씬 로딩
        var scene = SceneManager.LoadSceneAsync(type);
        scene.allowSceneActivation = false;
        do
        {
            yield return delay;
        } while (scene.progress < 0.9f);

        yield return untilTransition;
        scene.allowSceneActivation = true;

        transition.FadeIn(fadeTime);
        yield return untilTransition;
        IsDone = true;
    }
}
