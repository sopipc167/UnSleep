using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadManager : MonoBehaviour
{
    public Transform[] playerPos;
    public Transform[] gomePos;
    public GameObject gome;
    public Gome gome_script;
    public Player player;
    public DiaEvent DE;
    public GameObject Scene1;
    public GameObject Scene4;
    public GameObject GameOver_749;
    public Image backGround;


    void Start()
    {
        Debug.Log("savePoint " + PlayerPrefs.GetInt("savePoint"));
        Debug.Log("isGameOver " + PlayerPrefs.GetInt("isGameOver"));

        

        if (PlayerPrefs.GetInt("isGameOver") == 1)
        {
            Scene1.SetActive(false);

            DE.next_flase = 707;
            DE.next_true = 706;
            DE.Fade.enabled = true;
            backGround.enabled = false;

            player.isSeven = true;
            player.targetPos = DE.playerPos[PlayerPrefs.GetInt("savePoint")].position;
            player.transform.position = DE.playerPos[PlayerPrefs.GetInt("savePoint")].position;
            DE.ob[1].SetActive(false);
            gome.SetActive(true);
            gome.transform.DOScaleX(-2.3f, 0.1f);
            gome.transform.DOScaleY(2.3f, 0.1f);
            gome_script.targetPos = DE.gomePos[PlayerPrefs.GetInt("savePoint")].position;
            gome_script.transform.position = DE.gomePos[PlayerPrefs.GetInt("savePoint")].position;
            player.isSeven = false;
            if (PlayerPrefs.GetInt("savePoint") == 1)
            {
                Color tmp = new Vector4(0, 0, 0, 0);
                backGround.enabled = true;
                backGround.color = tmp;
                Dialogue_Proceeder.instance.UpdateCurrentDiaID(742);
                DE.Dia[37].SetActive(true);
                DE.Dia[38].SetActive(true);
                DE.Dia[39].SetActive(true);
                player.transform.eulerAngles = new Vector3(0, 180, 0);
                gome.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if(PlayerPrefs.GetInt("savePoint") == 2)
            {
                DE.Move(2, new Vector3(7.74f, -1.86f, 0), new Vector3(0, 0, -90));
                DE.Move(15, new Vector3(7.74f, -1.86f, 0), new Vector3(0, 0, -90));
                DE.ob[4].SetActive(false);
                DE.ob[3].SetActive(true);
                Dialogue_Proceeder.instance.UpdateCurrentDiaID(749);
                Scene4.SetActive(true);
                GameOver_749.SetActive(true);
                player.transform.eulerAngles = new Vector3(0, 0, 0);
                gome.transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }


        PlayerPrefs.SetInt("isGameOver", 0);
    }
}
