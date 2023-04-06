using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour
{
    [Header("참조")]
    public Text currentText;
    public Text selectedText;
    public RelationshipManager manager;

    private GameObject textParent;

    private readonly string colleague = "직장 동료";
    private readonly string gf = "연인";
    private readonly string friends = "친구들";

    private void Awake()
    {
        textParent = currentText.transform.parent.gameObject;
    }

    public bool ChangeName(CharacterType type)
    {
        RelationshipManager.CurrentType = type;

        textParent.SetActive(true);
        switch (type)
        {
            case CharacterType.Colleague:
                if (manager.IsClear(type))
                {
                    currentText.text = "아니, 도문씨 여기서 뭘 하고 있나요? 이런 쇠사슬에 같이 엉켜 있으니까 몸이 안 움직이죠..\n읏차.. 자, 이제 몸을 움직여보세요.";
                    return false;
                }
                else
                {
                    currentText.text = colleague;
                    selectedText.text = colleague;
                    return true;
                }
            case CharacterType.GF:
                if (manager.IsClear(type))
                {
                    currentText.text = "오빠.. 나 무서워.. 여기 어디야?\n저쪽에서 빛이 난 것 같았는데...";
                    return false;
                }
                else
                {
                    currentText.text = gf;
                    selectedText.text = gf;
                    return true;
                }
            case CharacterType.Friends:
                if (manager.IsClear(type))
                {
                    currentText.text = "뭐야, 도문아! 여기 갇혀있는거야?\n우리 함께 여기를 탈출하자! 아까 저기서 열쇠를 주웠어!";
                    return false;
                }
                else
                {
                    currentText.text = friends;
                    selectedText.text = friends;
                    return true;
                }
            default: return true;
        }
    }

    public void ResetText()
    {
        currentText.text = string.Empty;
        textParent.SetActive(false);
    }

    public void StartSceneA(Sprite sprite)
    {
        currentText.text = string.Empty;
        manager.StartSceneA(sprite);
    }
}
