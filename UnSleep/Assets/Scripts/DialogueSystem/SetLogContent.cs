using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SetLogContent : MonoBehaviour
{
    public Image char_img; //초상화
    public Text name_; //이름
    public Text context; //대사



    public void setItemInfo(Sprite sprite, string n, string c)
    {
        char_img.sprite = sprite;
        name_.text = n;
        context.text = c;
    }

    public void setItemInfo(string c) //나레이션이면 대사만 출력
    {
        char_img.sprite = null;
        char_img.gameObject.SetActive(false);
        name_.gameObject.SetActive(false);
        context.text = c;
    }


}
