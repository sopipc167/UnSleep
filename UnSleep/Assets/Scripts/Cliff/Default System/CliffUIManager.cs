using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CliffUIManager : MonoBehaviour
{
    [Header("상호작용 수 텍스트")]
    public Text interactNumText;

    [Header("상호작용 수 배경")]
    public Image interactNumBackground;
    public Color defaultBackground;
    public Color canDeleteColor;
    [Tooltip("깜박이는데 걸리는 시간"), Range(0.1f, 2f)]
    public float routineTime;
    private bool deleteFalg = false;

    [Header("상호작용 이미지")]
    public Image interactBackgound;
    public Image interactSprite;

    [Header("기본 바탕 색")]
    public Color backgroundColor;

    [Header("상호작용 1개 이미지")]
    public Sprite[] defaultImages;

    [Header("모양만 다를 때"), Tooltip("순서: 원, 삼각, 가각, 오각")]
    public Sprite[] ShapeDiffImages;

    [Header("색만 다를 때"), Tooltip("순서: 빨강, 노랑, 초록, 파랑")]
    public Color[] colorDiff;

    [Header("색과 모양 모두 다를 때")]
    public Sprite allDiffImage;


    //[Header("현재 진행도 텍스트")]
    //public Text progressText;

    private float currentPercent = 0f;
    public int GetProgress { get => Mathf.RoundToInt(currentPercent); }


    private void Start()
    {
        ChangeTypeText(CliffCheckType.None);
        ChangeInteractNumText(0);
        //ChangeProgressText(0);
    }

    //Color와 Shape일 때만 tile을 인자로
    public void ChangeTypeText(CliffCheckType type, CliffTile tile = null)
    {
        switch (type)
        {
            case CliffCheckType.Color:
                interactSprite.gameObject.SetActive(false);
                interactBackgound.color = colorDiff[(int)tile.type.color - 1];
                return;
            case CliffCheckType.Shape:
                interactSprite.gameObject.SetActive(true);
                interactSprite.rectTransform.sizeDelta = new Vector2(110f, 128f);
                interactSprite.sprite = ShapeDiffImages[(int)tile.type.shape - 1];
                interactBackgound.color = backgroundColor;
                break;
            case CliffCheckType.All:
                interactSprite.gameObject.SetActive(true);
                interactSprite.rectTransform.sizeDelta = new Vector2(110f, 128f);
                interactSprite.sprite = defaultImages[((int)tile.type.color - 1) * 4 + ((int)tile.type.shape - 1)];
                interactBackgound.color = backgroundColor;
                break;
            case CliffCheckType.AllDiff:
                interactSprite.gameObject.SetActive(true);
                interactSprite.rectTransform.sizeDelta = new Vector2(110f, 110f);
                interactSprite.sprite = allDiffImage;
                interactBackgound.color = backgroundColor;
                break;
            default:
                interactSprite.gameObject.SetActive(false);
                interactBackgound.color = backgroundColor;
                break;
        }
    }

    public void ChangeInteractNumText(int num)
    {
        interactNumText.text = num.ToString();

        if (num == 0 && deleteFalg)
        {
            deleteFalg = false;
            StopAllCoroutines();
            interactNumBackground.color = defaultBackground;
        }
        else if (num == 3 && !deleteFalg)
        {
            deleteFalg = true;
            StartCoroutine(DeleteCoroutine());
        }
    }

    private IEnumerator DeleteCoroutine()
    {
        Color diffColor = defaultBackground - canDeleteColor;
        diffColor /= routineTime;
        while (true)
        {
            while (interactNumBackground.color.r < canDeleteColor.r)
            {
                interactNumBackground.color -= diffColor * Time.deltaTime;
                yield return null;
            }
            interactNumBackground.color = canDeleteColor;

            while (interactNumBackground.color.r > defaultBackground.r)
            {
                interactNumBackground.color += diffColor * Time.deltaTime;
                yield return null;
            }
            interactNumBackground.color = defaultBackground;
        }
    }
    

    public void ChangeProgress(float percent)
    {
        currentPercent += percent;
        //progressText.text = "진행도: " + (Mathf.RoundToInt(currentPercent)).ToString() + "%";
    }


    //아래는 Button function
    public void OnClickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
