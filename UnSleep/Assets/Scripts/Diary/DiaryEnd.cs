using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaryEnd : MonoBehaviour
{
    public Auto auto;
    //public Sprite illust;
    public GameObject ending;


    void Start()
    {
        // 일단 잘 있어요가 클리어 상황에서 실행되게 했는데, 나중에는 잘 있어요를 클리어하고 나온 후에만 실행되도록 해야 함.
        if (SaveDataManager.Instance.Progress == 20)
            StartCoroutine(Ending());
    }

    

    IEnumerator Ending()
    {
        // 후일담 출력할 때까지 대기

        yield return new WaitForSeconds(7f);


        // 페이지 넘기기 시작 
        auto.AutoFlip = true;
        yield return new WaitForSeconds(2f);

        // 페이지가 4장정도 넘어가면 줌 아웃 시작
        RectTransform rect = GetComponent<RectTransform>();

        for (float xy = 1f; xy > 0.75f; xy -= 0.0001f)
        {
            rect.localScale = new Vector3(xy, xy);
            yield return null;
        }

    }

    public void afterClickMsg()
    {
        StartCoroutine(Illustrate());
    }

    // 편지 내용은.. 어떡하지..? 일단 편지 내용을 뭉개서 넣어놨음
    IEnumerator Illustrate()
    {
        yield return new WaitForSecondsRealtime(5f);

        ending.transform.GetChild(0).gameObject.SetActive(true);
        Image image = ending.transform.GetChild(0).GetComponent<Image>();

        // 일러스트를 페이드 인하며 표시
        for (float alpha = 0f; alpha < 1f; alpha += 0.001f)
        {
            image.color = new Color(1f, 1f, 1f, alpha);
            yield return new WaitForSeconds(0.01f);
        }


        Text text = ending.transform.GetChild(1).GetComponent<Text>();
        // Fin 글씨를 표시
        for (float alpha = 0f; alpha < 1f; alpha += 0.01f)
        {
            Color fadeColor = text.color;
            text.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha);
            yield return null;
        }
    }
    

}
