using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScroller : MonoBehaviour
{
    public SpriteRenderer[] tiles;
    public Sprite[] groundImg;
    public float speed;
    float Speed;
    public float PosY;
    void Start()
    {
        temp = tiles[0];
        Speed = speed;
    }
    SpriteRenderer temp;

    void Update()
    {
        for(int i = 0; i < tiles.Length; i++)
        {
            if(-15 >= tiles[i].transform.position.x)
            {
                for(int q = 0; q < tiles.Length; q++)
                {
                    if (temp.transform.position.x < tiles[q].transform.position.x)
                        temp = tiles[q];
                }
                tiles[i].transform.position = new Vector2(temp.transform.position.x + 7, PosY);
                tiles[i].sprite = groundImg[Random.Range(0, groundImg.Length)];
            }
        }
        for(int i = 0; i < tiles.Length; i++)
        {
            tiles[i].transform.Translate(new Vector2(-1, 0) * Time.deltaTime * speed);
        }
    }

    public void SpeedBack()
    {
        speed = Speed;
    }
}
