using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAText : MonoBehaviour
{
    [Header("참조")]
    public RelationshipManager manager;
    public RandPosText randPosText;

    [Header("참조")]
    public SpriteRenderer domun;
    public SpriteRenderer opposite;

    private TextSizeButton[] buttons = new TextSizeButton[2];

    private void Awake()
    {
        buttons[0] = transform.GetChild(0).GetComponent<TextSizeButton>();
        buttons[0].RefreshSize();
        buttons[1] = transform.GetChild(1).GetComponent<TextSizeButton>();
    }

    public void Refresh()
    {
        buttons[1].RefreshSize();
    }

    public void ResetData()
    {
        domun.color = new Color(1f, 1f, 1f);
        opposite.color = new Color(1f, 1f, 1f);
    }

    public void OnClickDomin()
    {
        opposite.color = new Color(0.2f, 0.2f, 0.2f);
        randPosText.PrintText(manager.currentType, true);
        manager.StartSceneB();
    }

    public void OnClickOpposite()
    {
        domun.color = new Color(0.2f, 0.2f, 0.2f);
        randPosText.PrintText(manager.currentType, false);
        manager.StartSceneB();
    }
}
