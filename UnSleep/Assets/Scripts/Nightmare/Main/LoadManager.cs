using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LoadManager : MonoBehaviour
{
    public Transform[] playerPos;
    public Transform[] gomePos;
    public GameObject gome;
    public Gome gome_script;
    public Player player;

    void Start()
    {
        if (PlayerPrefs.GetInt("isGameOver") == 1)
        {
            Debug.Log("savePoint " + PlayerPrefs.GetInt("savePoint"));
            player.isSeven = true;
            player.targetPos = playerPos[PlayerPrefs.GetInt("savePoint")].position;
            player.transform.position = playerPos[PlayerPrefs.GetInt("savePoint")].position;
            gome.SetActive(true);
            gome.transform.eulerAngles = new Vector3(0, 0, 0);
            gome.transform.DOScaleX(-2.3f, 0.1f);
            gome.transform.DOScaleY(2.3f, 0.1f);
            gome_script.targetPos = gomePos[PlayerPrefs.GetInt("savePoint")].position;
            gome_script.transform.position = gomePos[PlayerPrefs.GetInt("savePoint")].position;
            player.isSeven = false;
            PlayerPrefs.SetInt("isGameOver", 0);
        }

    }

    void Update()
    {
        
    }
}
