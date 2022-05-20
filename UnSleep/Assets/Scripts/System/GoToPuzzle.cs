using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoToPuzzle : MonoBehaviour
{
    [Header("참조")]
    public Text text;
    public Image background;

    [Header("시계탑")]
    public string clockText;
    public Color clockColor;

    [Header("화산")]
    public string volcanoText;
    public Color volcanoColor;

    [Header("호수")]
    public string lakeText;
    public Color lakeColor;

    [Header("동굴")]
    public string caveText;
    public Color caveColor;

    [Header("절벽")]
    public string cliffText;
    public Color cliffColor;

    private float startDelay;
    private float endDelay;
    private SceneType sceneType;


    public void GoPuzzle(SceneType type, float startDelay = 0f, float endDelay = 2f)
    {
        sceneType = type;
        switch (sceneType)
        {
            case SceneType.Volcano:
                text.text = volcanoText;
                background.color = volcanoColor;
                break;
            case SceneType.Clock:
                text.text = clockText;
                background.color = clockColor;
                break;
            case SceneType.Lake:
                text.text = lakeText;
                background.color = lakeColor;
                break;
            case SceneType.Cave:
                text.text = caveText;
                background.color = caveColor;
                break;
            case SceneType.Cliff:
                text.text = cliffText;
                background.color = cliffColor;
                break;
            default:
                break;
        }
        this.startDelay = startDelay;
        this.endDelay = endDelay < 2f ? 2f : endDelay;
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        StartCoroutine(GoCoroutine());
    }

    private IEnumerator GoCoroutine()
    {
        if (startDelay != 0f)
        {
            yield return new WaitForSeconds(startDelay);
        }

        yield return new WaitForSeconds(endDelay);

        SceneChanger.ChangeScene(sceneType);
    }
}
