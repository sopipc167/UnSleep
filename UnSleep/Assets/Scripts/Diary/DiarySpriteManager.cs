using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DiarySprite
{
    // 등장인물
    public Sprite[] charSpr;

    // 시작 버튼
    public Sprite startSpr;
}
public class DiarySpriteManager : MonoBehaviour
{
    public DiarySprite[] diarySprites;

    public DiarySprite GetDiarySprite(int epinum)
    {
        return diarySprites[epinum];
    }

}
