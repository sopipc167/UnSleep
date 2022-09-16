using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelationshipManager : MonoBehaviour
{
    [Header("대기 화면")]
    public SelectManager characterCnavas;
    public GameObject dialogeTextCanvas;

    [Header("게임 화면 A, B 공통")]
    public ScaleManager scale;
    public TargetCharacter character2;
    public GameObject scaleTextCanvas;

    [Header("게임 화면 A")]
    public SceneAText selectedCanvas;

    [Header("게임 화면 B")]
    public GameObject dialogueCanvas;

    [Header("클리어 버튼")]
    public GameObject claerButton;


    internal CharacterType currentType;

    private bool[] clearFlags = { false, false, false };

    private void Start()
    {
        StartScenePause();
    }

    public void StartScenePause()
    {
        characterCnavas.gameObject.SetActive(true);
        dialogeTextCanvas.SetActive(true);

        scale.gameObject.SetActive(false);
        character2.gameObject.SetActive(false);
        scaleTextCanvas.SetActive(false);
        selectedCanvas.gameObject.SetActive(false);
        dialogueCanvas.SetActive(false);

        if (AllClear()) claerButton.SetActive(true);
        Debug.Log("sd");
    }

    public void StartSceneA(Sprite sprite)
    {
        character2.ChangeSprite(sprite);

        characterCnavas.gameObject.SetActive(false);
        dialogeTextCanvas.SetActive(false);

        scale.gameObject.SetActive(true);
        scale.ResetData();
        character2.gameObject.SetActive(true);
        scaleTextCanvas.SetActive(true);

        selectedCanvas.gameObject.SetActive(true);
        selectedCanvas.Refresh();

        dialogueCanvas.SetActive(false);
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
        
    }
}
