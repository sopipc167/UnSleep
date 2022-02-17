using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoleGenerator : MonoBehaviour
{
    public GameObject[] hole_012;
    public Sprite[] hole_imgs;
    public int holecnt;
    private float gx;
    private float gy;
    private int gi;
    // Start is called before the first frame update
    void Start()
    {
        RandomHoleGenerate();
    }

   void RandomHoleGenerate()
    {
        if (holecnt == 1)
        {
            gi = Random.Range(0, 4);
            gy = Random.Range(400f, 500f) - 540f;
            gx = Random.Range(700f, 1220f) - 960f;
            hole_012[0].transform.localPosition = new Vector2(gx, gy);
            hole_012[0].GetComponent<Image>().sprite = hole_imgs[gi];
        }
        else if (holecnt == 2)
        {
            gi = Random.Range(0, 4);
            gy = Random.Range(400f, 500f) - 540f;
            gx = Random.Range(-400f, -340f);
            Debug.Log(gx);
            hole_012[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(gx, gy);
            hole_012[0].GetComponent<Image>().sprite = hole_imgs[gi];


            gi = Random.Range(0, 4);
            gy = Random.Range(400f, 500f) - 540f;
            gx = Random.Range(1160f, 1400f) - 960f;
            hole_012[1].transform.localPosition = new Vector2(gx, gy);
            hole_012[1].GetComponent<Image>().sprite = hole_imgs[gi];


        }
        else if (holecnt == 3)
        {
            gi = Random.Range(0, 4);
            gy = Random.Range(400f, 500f) - 540f;
            gx = Random.Range(300f, 472f) - 960f;
            hole_012[0].transform.localPosition = new Vector2(gx, gy);
            hole_012[0].GetComponent<Image>().sprite = hole_imgs[gi];

            gi = Random.Range(0, 4);
            gy = Random.Range(400f, 500f) - 540f;
            gx = Random.Range(872f, 1048f) - 960f;
            hole_012[1].transform.localPosition = new Vector2(gx, gy);
            hole_012[1].GetComponent<Image>().sprite = hole_imgs[gi];

            gi = Random.Range(0, 4);
            gy = Random.Range(400f, 500f) - 540f;
            gx = Random.Range(1448f, 1620f) - 960f;
            hole_012[2].transform.localPosition = new Vector2(gx, gy);
            hole_012[2].GetComponent<Image>().sprite = hole_imgs[gi];


        }
    }
}
