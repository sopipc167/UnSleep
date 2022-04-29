using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnwindRibbon : StoryInteract
{
    public Sprite[] ribbons;

    private TextManager textManager;
    private bool clickFlag = false;
    private bool result = false;
    private Image image;
    private WaitForSeconds delay = new WaitForSeconds(0.1f);

    private void Start()
    {
        image = GetComponent<Image>();
        textManager = transform.parent.parent.parent.GetChild(0).GetComponent<TextManager>();
        transform.localPosition = new Vector3(-12.5f, -13f, 0f);
    }

    public override bool IsCompelete()
    {
        return result;
    }

    // Update is called once per frame
    void Update()
    {
        if (!clickFlag && Input.GetMouseButtonDown(0))
        {
            clickFlag = true;
            StartCoroutine(RibbonCoroutine());
        }

        if (textManager.dialogues_index == 11)
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator RibbonCoroutine()
    {
        int size = ribbons.Length;
        for (int i = 1; i < size; ++i)
        {
            image.sprite = ribbons[i];
            image.SetNativeSize();
            yield return delay;
        }
        result = true;
    }
}
