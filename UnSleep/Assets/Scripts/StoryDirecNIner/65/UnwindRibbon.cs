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
    private WaitForSeconds delay = new WaitForSeconds(0.1f);

    private void Start()
    {
        image = GetComponent<Image>();
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
        gameObject.SetActive(false);
    }
}
