using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCliffUI : MonoBehaviour
{
    public CliffUIManager manager;
    public Image mainSprite;
    public Image mainbg;
    public Image icwbg;
    public Text icText;
    public Color defaultColor;
    public Color changeColor;
    public Sprite[] sprites;



    protected WaitForSeconds delay = new WaitForSeconds(0.5f);

    protected void OnEnable()
    {
        StartCoroutine(ChangeUICoroutine());
    }

    protected void OnDisable()
    {
        StopAllCoroutines();
        icwbg.color = manager.defaultBackground;
    }

    protected virtual IEnumerator ChangeUICoroutine()
    {
        while (true)
        {
            icText.text = "0";
            mainSprite.gameObject.SetActive(false);
            mainbg.color = defaultColor;
            yield return delay;

            mainSprite.gameObject.SetActive(true);
            int i = 1;
            foreach (var item in sprites)
            {
                if (item == null)
                {
                    mainSprite.gameObject.SetActive(false);
                    mainbg.color = changeColor;
                }
                else
                {
                    mainSprite.sprite = item;
                }

                icText.text = i.ToString();
                yield return delay;
                ++i;
            }

            Color diffColor = manager.defaultBackground - manager.canDeleteColor;
            diffColor /= manager.routineTime;

            for (i = 0; i < 2; ++i)
            {
                while (icwbg.color.r < manager.canDeleteColor.r)
                {
                    icwbg.color -= diffColor * Time.deltaTime;
                    yield return null;
                }
                icwbg.color = manager.canDeleteColor;

                while (icwbg.color.r > manager.defaultBackground.r)
                {
                    icwbg.color += diffColor * Time.deltaTime;
                    yield return null;
                }
                icwbg.color = manager.defaultBackground;
            }
        }
    }
}
