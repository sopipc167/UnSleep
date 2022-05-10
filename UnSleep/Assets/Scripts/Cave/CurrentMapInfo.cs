using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CurrentMapInfo : MonoBehaviour
{

    public int i; //Row 탐색
    public int j; //Col 탐색

    public int save_i; //세이브지점 인덱스 
    public int save_j;

    public int route;
    public bool isTalk;
    public int talk_id;

    public bool isAudio;
    public string sound_position;
    public int sound_index;
    public float volume;
    public int sound_index_2;
    public float volume_2;

    public bool isObject;
    public int object_index;

    public bool isSave;
    public string mapcode;



    MapGenerator mapGenerator;
    AudioLeftRight audioLeftRight;
    ObjectManager objectManager;
    public TextManager textManager;

    public GameObject[] CavePrepabs; //임시구현
    public GameObject CAVE;

    public Canvas canvas;
    private GraphicRaycaster gr;
    private GameObject grresult;

    public GameObject BackButton;
    public Text Saving;

    public GameObject trasparentImg;

    // Start is called before the first frame update
    private void Awake()
    {
        mapGenerator = GetComponent<MapGenerator>();
        audioLeftRight = GetComponent<AudioLeftRight>();
        objectManager = GetComponent<ObjectManager>();
        objectManager.GetObjects(Dialogue_Proceeder.instance.CurrentEpiID);
        gr = canvas.GetComponent<GraphicRaycaster>();

    }

    void Start()
    {

        i = 0;
        j = 0;
        SetMapInfo(i, j);
    }

    // Update is called once per frame
    void Update()
    {
        if (textManager.DiaUI.activeSelf == false &&Input.GetMouseButtonDown(0))
        {
            var ped = new PointerEventData(null);
            ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(ped, results);

            if (results.Count > 0)
            {
                if (results[0].gameObject.CompareTag("Cavehole"))
                {
                    trasparentImg.SetActive(true);
                    grresult = results[0].gameObject;
                    Proceed();
                    //Debug.Log(results[0].gameObject.name);
                    objectManager.SetObjectFadeOff();

                }

            }
            else
            {
                grresult = null;
            }
        }

        if (i == 0 && j == 0 && BackButton.activeSelf)
            BackButton.SetActive(false);
        else if ((i != 0 || j != 0) && !BackButton.activeSelf)
            BackButton.SetActive(true);

    }

    public void SetMapInfo(int i, int j)
    {
       

        route = mapGenerator.Row[i].Col[j].route;
        isTalk = mapGenerator.Row[i].Col[j].isTalk;
        talk_id = mapGenerator.Row[i].Col[j].talk_id;
        isAudio = mapGenerator.Row[i].Col[j].isAudio;
        sound_position = mapGenerator.Row[i].Col[j].sound_position;
        sound_index = mapGenerator.Row[i].Col[j].sound_index;
        volume = mapGenerator.Row[i].Col[j].volume;
        sound_index_2 = mapGenerator.Row[i].Col[j].sound_index_2;
        volume_2 = mapGenerator.Row[i].Col[j].volume_2;
        isObject = mapGenerator.Row[i].Col[j].isObject;
        object_index = mapGenerator.Row[i].Col[j].object_index;

        mapcode = mapGenerator.Row[i].Col[j].mapcode;
        isSave = mapGenerator.Row[i].Col[j].isSave;



        if (isAudio)
        {
            if (sound_position.Equals("LR"))
                audioLeftRight.SetAudioLR(sound_index, volume, sound_index_2, volume_2);
            else
                audioLeftRight.SetAudio(sound_position, sound_index, volume);

        }
        else
            audioLeftRight.SetAudioMute();


        if (route == 999)
            audioLeftRight.FadeoutCave();

        if (isTalk)
        {
            if (!Dialogue_Proceeder.instance.AlreadyDone(talk_id))
            {

                if (Dialogue_Proceeder.instance.Satisfy_Condition(textManager.ReturnDiaConditions(talk_id)))
                {
                    Dialogue_Proceeder.instance.UpdateCurrentDiaID(talk_id);
                    textManager.Increasediaindex = true;
                    textManager.SetDiaInMap();
                }

            }


        }

        if (isObject)
        {
            objectManager.SetObject(object_index);
        }


        if (isSave)
        {
            save_i = i;
            save_j = j;
            StartCoroutine("AutoSaving");
        }

        mapGenerator.MapDic[mapcode].SetActive(true);
        trasparentImg.SetActive(false);




    }

    public void Proceed()
    {
        string cur_mapcode = i.ToString() + j.ToString();

        mapGenerator.MapDic[cur_mapcode].gameObject.GetComponent<Cave_Animation>().Ani_Proceed();
        Invoke("Proceed_half", 1f);
    }

    public void Proceed_half()
    {
        int nexti = 0;
        int nextj = j + 1;
        string cur_mapcode = i.ToString() + j.ToString();
        mapGenerator.MapDic[cur_mapcode].gameObject.GetComponent<Cave_Animation>().Ani_Reset();
        mapGenerator.MapDic[cur_mapcode].SetActive(false);


        for (int k = 0; k < i; k++)
        {
            int tmp = mapGenerator.Row[k].Col[j].route;
            if (tmp > 0)
                nexti += tmp;
        }


        if (grresult.name.Equals("0"))
            nexti += 0;
        else if (grresult.name.Equals("1"))
            nexti += 1;
        else if (grresult.name.Equals("2"))
            nexti += 2;

        grresult = null;


        i = nexti;
        j = nextj;

        SetMapInfo(i, j);

    }


    public void Back()
    {
        //int previ;
        //int prevj = j - 1;
        //int tmp = 0;
        string cur_mapcode = i.ToString() + j.ToString();

        trasparentImg.SetActive(true);
        objectManager.SetObjectFadeOff();
        mapGenerator.MapDic[cur_mapcode].gameObject.GetComponent<Cave_Animation>().Ani_Back();
        Invoke("Back_half", 0.75f);
    }

    public void Back_half()
    {
        int previ;
        int prevj = j - 1;
        int tmp = 0;
        string cur_mapcode = i.ToString() + j.ToString();

        for (previ = 0; previ < mapGenerator.Row.Length; previ++)
        {
            tmp += mapGenerator.Row[previ].Col[prevj].route;

            if (tmp > i)
                break;
        }


        mapGenerator.MapDic[cur_mapcode].SetActive(false);

        i = previ;
        j = prevj;

        SetMapInfo(i, j);

    }


    public void BacktoSavePoint()
    {
        string cur_mapcode = i.ToString() + j.ToString();
        mapGenerator.MapDic[cur_mapcode].SetActive(false);

        i = save_i;
        j = save_j;
        SetMapInfo(save_i, save_j);
    }


    IEnumerator AutoSaving()
    {

        for (float f = 0f; f < 1f; f += 0.1f)
        {
            yield return new WaitForSeconds(0.05f);

            Saving.color = new Color(1f, 1f, 1f, f);

        }

        for (float f = 1f; f > -0f; f -= 0.1f)
        {
            yield return new WaitForSeconds(0.05f);

            Saving.color = new Color(1f, 1f, 1f, f);

        }

        Saving.color = new Color(1f, 1f, 1f, 0f);
        yield return null;
    }
}
