using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteManager : MonoBehaviour
{
    private readonly string url = "/BackGround/";


    public void LoadImage(Image target, string name, string extension = ".png")
    {
        byte[] byteTexture = System.IO.File.ReadAllBytes(Application.streamingAssetsPath + url + name + extension);
        Texture2D texture = new Texture2D(0, 0);
        texture.LoadImage(byteTexture);

        Rect rect = new Rect(0, 0, texture.width, texture.height);
        target.sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
    }

    public void LoadImageLegacy(Image target, string _url, string name, string extension = ".png")
    {
        byte[] byteTexture = System.IO.File.ReadAllBytes(_url + name + extension);
        Texture2D texture = new Texture2D(0, 0);
        texture.LoadImage(byteTexture);

        Rect rect = new Rect(0, 0, texture.width, texture.height);
        target.sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
    }

    // legacy
    public void LoadImage(Image target, int index)
    {
        switch (index)
        {
            case 0:
                LoadImage(target, "classroom");
                break;
            case 1:
                LoadImage(target, "school");
                break;
            case 2:
                LoadImage(target, "livingroom");
                break;
            case 3:
                LoadImage(target, "room_eve");
                break;
            case 4:
                LoadImage(target, "room_mor");
                break;
            case 5:
                LoadImageLegacy(target, "Assets/Sprites/Illustration/18/", "18-1");
                break;
            case 6:
                LoadImageLegacy(target, "Assets/Sprites/Illustration/18/", "18-2");
                break;
            case 7:
                LoadImageLegacy(target, "Assets/Sprites/Illustration/18/", "18-3");
                break;
            default:
                LoadImageLegacy(target, "Assets/Sprites/", "fade");
                break;
        }
    }
}
