using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NightMareManager : MonoBehaviour
{
    public static NightMareManager instance;

    public GameObject player;
    public Player player_s;
    public TextManager TM;

    //층간소음
    public GameObject openning;
    public GameObject noiseManager;
    public GameObject noiseObject;

    //층간소음 Scene1
    public GameObject[] Scene1;

    //층간소음 Scene2
    public GameObject Scene2;
    public Slider gauge;
    public Button skip;
    public Text skip_text;

    //층간소음 Scene3
    public GameObject Scene3;
    public CameraManager camera;

    //7세
    public GameObject Seven;
    public Image BackGround;
    public GameObject loadManager;
    public GameObject diaEvent;

    //Test
    public int Case;

    void Start()
    {
        instance = this;
        if (Dialogue_Proceeder.instance.CurrentEpiID == 6)
        {
            endSeven();
            noiseManager.SetActive(true);
            startNoise();
            startScene1();
        }
        else if(Dialogue_Proceeder.instance.CurrentEpiID == 1)
        {
            noiseManager.SetActive(false);
            endScene1();
            endScene2();
            endScene3();
            StartSeven();
        }

    }

    void Update()
    {
        //TestCode
        switch (Case)
        {
            case 0:
                startNoise();
                break;
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

    public void startNoise()
    {
        openning.SetActive(true);
        noiseObject.SetActive(true);
    }


    public void startScene1()
    {
        TM.isNoise = true;
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
        //gauge.enabled = true;

        //skip버튼
        skip.enabled = true;
        skip.image.enabled = true;
        skip_text.enabled = true;

        player_s.isMiniGame = true;
        player.transform.localScale = new Vector3(0.35f, 0.35f, 0);
        player_s.Noise.enabled = true;
        player_s.isStop = false;
        player.SetActive(true);
    }

    public void endScene2()
    {
        skip.enabled = false;
        skip.image.enabled = false;
        skip_text.enabled = false;

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
        TM.isNoise = false;
        TM.isSeven = true;
        player_s.Seven.enabled = true;
        player.SetActive(true);
        Seven.SetActive(true);
        loadManager.SetActive(true);
        diaEvent.SetActive(true);
        Color tmp = BackGround.color;
        tmp.a = 255;
        BackGround.color = tmp;
    }

    public void endSeven()
    {
        TM.isSeven = false;
        loadManager.SetActive(false);
        diaEvent.SetActive(false);
        player_s.Seven.enabled = false;
        player.SetActive(false);
        Seven.SetActive(false);
    }
}
