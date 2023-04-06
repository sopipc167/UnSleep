using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RelationshipManager : MonoBehaviour
{
    [Header("대기 화면")]
    public SelectManager characterCnavas;
    public Sprite[] clearCharacters;
    public GameObject dialogeTextCanvas;

    [Header("게임 화면 A, B 공통")]
    public ScaleManager scale;
    public TargetCharacter character2;
    public GameObject scaleTextCanvas;
    public GameObject characterText;

    [Header("게임 화면 A")]
    public SceneAText selectedCanvas;

    [Header("게임 화면 B")]
    public GameObject dialogueCanvas;
    public RandPosText dialogueTextCanvas;

    [Header("클리어 버튼")]
    public GameObject claerButton;

    [Header("사운드")]
    public AudioClip bgm;
    public AudioClip clearSound;


    public static CharacterType CurrentType;
    private readonly Image[] characters = new Image[3];
    private readonly bool[] clearFlags = { false, false, false };

    private void Awake()
    {
        SoundManager.Instance.PlayBGM(bgm);
        for (int i = 0; i < characterCnavas.transform.childCount; ++i)
        {
            characters[i] = characterCnavas.transform.GetChild(i).GetComponent<Image>();
        }
    }

    private void Start()
    {
        characterCnavas.gameObject.SetActive(true);
        dialogeTextCanvas.SetActive(true);

        scale.gameObject.SetActive(false);
        scaleTextCanvas.SetActive(false);
        characterText.SetActive(false);

        character2.gameObject.SetActive(false);
        selectedCanvas.gameObject.SetActive(false);
        dialogueCanvas.SetActive(false);
    }

    public void PlayClearSound() => SoundManager.Instance.PlaySE(clearSound);

    public void ClearPhase()
    {
        characterCnavas.gameObject.SetActive(true);
        dialogeTextCanvas.SetActive(true);
        character2.gameObject.SetActive(false);
        selectedCanvas.gameObject.SetActive(false);
        dialogueCanvas.SetActive(false);

        if (AllClear()) claerButton.SetActive(true);
    }

    public void AfterAnimationProcess()
    {
        scale.gameObject.SetActive(false);
        scaleTextCanvas.SetActive(false);
        characterText.SetActive(false);
    }

    public void StartSceneA(Sprite sprite)
    {
        character2.ChangeSprite(sprite);

        characterCnavas.gameObject.SetActive(false);
        dialogeTextCanvas.SetActive(false);

        characterText.SetActive(true);
        scale.gameObject.SetActive(true);
        scale.ResetData();
        character2.gameObject.SetActive(true);
        scaleTextCanvas.SetActive(true);

        selectedCanvas.gameObject.SetActive(true);
        selectedCanvas.Refresh();

        dialogueCanvas.SetActive(false);
        dialogueTextCanvas.SetInitText();
    }

    public void StartSceneB()
    {
        selectedCanvas.gameObject.SetActive(false);
        dialogueCanvas.SetActive(true);
    }

    public void BackToSceneA()
    {
        dialogueCanvas.SetActive(false);
        selectedCanvas.gameObject.SetActive(true);
    }

    public void CharacterClear(CharacterType type)
    {
        switch (type)
        {
            case CharacterType.Colleague:
                clearFlags[0] = true;
                break;
            case CharacterType.GF:
                clearFlags[1] = true;
                break;
            case CharacterType.Friends:
                clearFlags[2] = true;
                break;
            default:
                break;
        }
        int idx = (int)type;
        characters[idx].sprite = clearCharacters[idx];
        characters[idx].SetNativeSize();
    }

    public bool IsClear(CharacterType type)
    {
        switch (type)
        {
            case CharacterType.Colleague: return clearFlags[0];
            case CharacterType.GF: return clearFlags[1];
            case CharacterType.Friends: return clearFlags[2];
            default: return false;
        }
    }

    private bool AllClear()
    {
        if (clearFlags[0] && clearFlags[1] && clearFlags[2]) return true;
        else return false;
    }

    public void OnClickClear()
    {
        // 씬이동
        Dialogue_Proceeder.instance.UpdateCurrentDiaIDPlus1();
        SceneChanger.ChangeScene(SceneType.Dialogue);
    }
}
