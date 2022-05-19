using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryEvent45 : StoryInteract
{
    public Button button;

    public Image doorImg;
    public Image domunImg;
    public Image domunEffectImg;
    public Image eviImg;
    public Image eviEffectImg;

    public Sprite door;
    public Sprite[] evi;
    public Sprite[] domun;
    public Sprite eviEffect;
    public Sprite domunEffect;

    private bool isClick = false;
    private Dialogue_Proceeder dp;

    private void Awake()
    {
        dp = Dialogue_Proceeder.instance;
    }

    private void OnEnable()
    {
        isClick = false;
    }

    public override bool IsCompelete()
    {
        return isClick;
    }

    public void OnclickCharacter()
    {
        isClick = true;
        button.interactable = false;
        doorImg.sprite = door;
        eviImg.transform.gameObject.SetActive(true);
        StartCoroutine(EventCoroutine());
    }

    private IEnumerator EventCoroutine()
    {
        yield return new WaitUntil(() => dp.CurrentDiaIndex == 2);
        eviImg.sprite = evi[0];
        domunImg.sprite = domun[0];
        eviEffectImg.transform.gameObject.SetActive(true);
        domunEffectImg.transform.gameObject.SetActive(true);

        yield return new WaitUntil(() => dp.CurrentDiaIndex == 4);
        eviImg.sprite = evi[1];
        domunImg.sprite = domun[1];

        yield return new WaitUntil(() => dp.CurrentDiaIndex == 6);
        domunEffectImg.sprite = domunEffect;

        yield return new WaitUntil(() => dp.CurrentDiaIndex == 8);
        eviEffectImg.sprite = eviEffect;

        yield return new WaitUntil(() => dp.CurrentDiaID == 4507);
        gameObject.SetActive(false);
    }
}
