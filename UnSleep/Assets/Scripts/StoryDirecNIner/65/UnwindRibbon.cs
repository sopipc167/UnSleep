using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnwindRibbon : StoryInteract
{
    public Sprite[] ribbons;

    private bool clickFlag = false;
    private bool result = false;
    private Image image;
    private Dialogue_Proceeder dp;
    private WaitForSeconds delay = new WaitForSeconds(0.1f);

    private void Start()
    {
        dp = Dialogue_Proceeder.instance;
        image = GetComponent<Image>();
        transform.localPosition = new Vector3(-12.5f, -13f, 0f);
    }

    private void OnEnable()
    {
        result = false;
        image.sprite = ribbons[0];
        image.SetNativeSize();
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

        if (dp.CurrentDiaIndex == 11)
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
