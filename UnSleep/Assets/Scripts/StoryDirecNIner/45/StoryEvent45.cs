using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryEvent45 : StoryInteract
{
    private TextManager textManager;
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

    public override bool IsCompelete()
    {
        return isClick;
    }

    private void Start()
    {
        textManager = transform.parent.parent.parent.GetChild(0).GetComponent<TextManager>();
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
        yield return new WaitUntil(() => textManager.dialogues_index == 2);
        eviImg.sprite = evi[0];
        domunImg.sprite = domun[0];
        eviEffectImg.transform.gameObject.SetActive(true);
        domunEffectImg.transform.gameObject.SetActive(true);

        yield return new WaitUntil(() => textManager.dialogues_index == 4);
        eviImg.sprite = evi[1];
        domunImg.sprite = domun[1];

        yield return new WaitUntil(() => textManager.dialogues_index == 6);
        domunEffectImg.sprite = domunEffect;

        yield return new WaitUntil(() => textManager.dialogues_index == 8);
        eviEffectImg.sprite = eviEffect;

        yield return new WaitUntil(() => Dialogue_Proceeder.instance.CurrentDiaID == 4507);
        gameObject.SetActive(false);
    }
}
