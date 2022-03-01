using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ObjectManager : MonoBehaviour
{
    public Transform OBJECT;
    public GameObject[] cave_object;
    public GameObject[] objects;
    private int objectsCnt;

    public GameObject GearUI65;
    GameObject GearUI;

    private void Awake()
    {
        //GetObjects(9);
        SetObjectOff();
    }

    public void GetObjects(int epi_num)
    {
        GameObject rootObject;

        if (epi_num == 9)
            rootObject = MonoBehaviour.Instantiate(cave_object[0]);
        else if (epi_num == 19)
            rootObject = MonoBehaviour.Instantiate(cave_object[1]);
        else if (epi_num == 27)
            rootObject = MonoBehaviour.Instantiate(cave_object[2]);
        else if (epi_num == 50)
            rootObject = MonoBehaviour.Instantiate(cave_object[3]);
        else if (epi_num == 56)
            rootObject = MonoBehaviour.Instantiate(cave_object[4]);
        else if (epi_num == 65)
        {
            rootObject = MonoBehaviour.Instantiate(cave_object[5]);
            GearUI = MonoBehaviour.Instantiate(GearUI65);
            GearUI.transform.SetParent(OBJECT);
            //GearUI.transform.position = OBJECT.position;
            GearUI.SetActive(false);
        }
        else if (epi_num == 70)
            rootObject = MonoBehaviour.Instantiate(cave_object[6]);
        else if (epi_num == 80)
            rootObject = MonoBehaviour.Instantiate(cave_object[7]);
        else 
            rootObject = MonoBehaviour.Instantiate(cave_object[0]);





        rootObject.transform.SetParent(OBJECT);
        rootObject.transform.position = OBJECT.position;
        objectsCnt = rootObject.transform.childCount;
        objects = new GameObject[objectsCnt];
        for (int i = 0; i < objectsCnt; i++)
        {
            objects[i] = rootObject.transform.GetChild(i).gameObject;
        }
    }

    public void SetObject(int idx)
    {
        objects[idx].SetActive(true);
        objects[idx].GetComponent<CanvasGroup>().alpha = 1f;
        Debug.Log("켜짐");
    }

    public void SetObjectOff()
    {
        for (int i=0; i < objectsCnt; i++)
        {
            if (objects[i].activeSelf)
            {
                objects[i].SetActive(false);

            }
        }

    }

    public void SetObjectFadeOff()
    {
        CanvasGroup ObjectCG;
        for (int i = 0; i < objectsCnt; i++)
        {
            if (objects[i].activeSelf)
            {
                ObjectCG = objects[i].GetComponent<CanvasGroup>();
                ObjectCG.DOFade(0f, 0.5f);
                Debug.Log("흐려짐");

            }
        }
        Invoke("SetObjectOff", 0.5f);
    }


    public void OnGearUI()
    {
        GearUI.SetActive(true);
    }
}
