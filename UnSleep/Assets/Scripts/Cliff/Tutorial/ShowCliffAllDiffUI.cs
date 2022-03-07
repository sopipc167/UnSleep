using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCliffAllDiffUI : ShowCliffUI
{
    protected override IEnumerator ChangeUICoroutine()
    {
        while (true)
        {
            icText.text = "0";
            mainSprite.gameObject.SetActive(false);
            yield return delay;

            mainSprite.gameObject.SetActive(true);

            mainSprite.rectTransform.sizeDelta = new Vector2(110f, 128f);
            mainSprite.sprite = sprites[0];
            icText.text = "1";
            yield return delay;

            mainSprite.rectTransform.sizeDelta = new Vector2(110f, 110f);
            for (int i = 1; i < sprites.Length; )
            {
                mainSprite.sprite = sprites[i];
                ++i;
                icText.text = i.ToString();
                yield return delay;
            }

            Color diffColor = manager.defaultBackground - manager.canDeleteColor;
            diffColor /= manager.routineTime;

            for (int i = 0; i < 2; ++i)
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
