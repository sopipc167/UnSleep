using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaryTableObject : MonoBehaviour
{
    private Image image;
    public Sprite[] sprites;

    void Start()
    {
        image = GetComponent<Image>();

        switch(SaveDataManager.Instance.Progress)
        {
            case int n when (0 <= n && n <= 2): image.sprite = sprites[0]; break;
            case 3: image.sprite = sprites[1]; break;
            case int n when (3 < n && n <= 5): image.sprite = sprites[2]; break;
            case int n when (5 < n && n <= 14): image.sprite = sprites[3]; break;
            case int n when (n > 14): image.sprite = sprites[4]; break;
        }
    }


}
