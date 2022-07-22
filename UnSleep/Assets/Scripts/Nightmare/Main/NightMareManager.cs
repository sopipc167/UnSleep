﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NightMareManager : MonoBehaviour
{
    public static NightMareManager instance;

    public GameObject player;
    public Player player_s;

    //층간소음 Scene1
    public GameObject[] Scene1;

    //층간소음 Scene2
    public GameObject Scene2;
    public Slider gauge;

    //층간소음 Scene3
    public GameObject Scene3;
    public CameraManager camera;

    //7세
    public GameObject Seven;
    public Image BackGround;

    //Test
    public int Case;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        //TestCode
        switch (Case)
        {
            case 1:
                startScene1();
                break;
            case 2:
                endScene1();
                break;
            case 3:
                startScene2();
                break;
            case 4:
                endScene2();
                break;
            case 5:
                StartScene3();
                break;
            case 6:
                endScene3();
                break;
            case 7:
                StartSeven();
                break;
            case 8:
                endSeven();
                break;
        }
    }


    public void startScene1()
    {
        player.SetActive(false);
        for(int i = 0; i < Scene1.Length; i++)
        {
            Scene1[i].SetActive(true);
        }
    }

    public void endScene1()
    {
        for(int i = 0; i < Scene1.Length; i++)
        {
            Scene1[i].SetActive(false);
        }
    }

    public void startScene2()
    {
        camera.isMiniGame = true;
        Scene2.SetActive(true);
        player_s.isMiniGame = true;
        player.transform.localScale = new Vector3(0.35f, 0.35f, 0);
        player_s.Noise.enabled = true;
        player.SetActive(true);
    }

    public void endScene2()
    {
        gauge.enabled = false;
        player_s.Noise.enabled = false;
        camera.isMiniGame = false;
        Scene2.SetActive(false);
        player_s.isMiniGame = false;
        player.SetActive(false);
    }

    public void StartScene3()
    {
        Scene3.SetActive(true);
        camera.isThree = true;
    }

    public void endScene3()
    {
        Scene3.SetActive(false);
        camera.isThree = false;
    }

    public void StartSeven()
    {
        player_s.Seven.enabled = true;
        player.SetActive(true);
        Seven.SetActive(true);
        Color tmp = BackGround.color;
        tmp.a = 255;
        BackGround.color = tmp;
    }

    public void endSeven()
    {
        player_s.Seven.enabled = false;
        player.SetActive(false);
        Seven.SetActive(false);
    }
}
