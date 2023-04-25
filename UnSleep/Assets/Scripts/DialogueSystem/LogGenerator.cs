using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LogGenerator : MonoBehaviour
{
    public TextManager textManager;
    public GameObject logUI; //로그UI
    public GameObject logPrefab;
    public GameObject Content;


    private Dictionary<int, DialogueEvent> diaDic;
    private Dictionary<int, Sprite[]> porDic;
    private Dictionary<int, string> nameDic = new Dictionary<int, string>() //이름 딕셔너리. 캐릭터 id가 있는 주요 인물들
    {
        {1000,"잠재우미" },{1001,"도문"}, {1002,"어머니"}, {1003, "아버지"},
        {1004, "재준"}, {1005, "장현"}, {1006, "새나"}, {1007, "이비"},
        {1008, "구광일"}, {1009, "고준일"}
    };


    void Start()
    {
        diaDic = textManager.DiaDic;
        porDic = textManager.getPorDic();
    }

    public void turnOnLog()
    {
        createLog();
        logUI.SetActive(true);
        Content.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0); // 스크롤 항상 최하단부터.
    }

    public void turnOffLog()
    {
        logUI.SetActive(false);
        for (int i = 0; i < Content.transform.childCount; i++) //로그 프리팹 모두 삭제
        {
            Destroy(Content.transform.GetChild(i).gameObject);
        }
    }

    private void createLog()
    {
        List<int> targetList = Dialogue_Proceeder.instance.getLastCompleteConditionRange();
        targetList.ForEach(id => createLogItem(id));
        createLogItem(Dialogue_Proceeder.instance.CurrentDiaID, Dialogue_Proceeder.instance.CurrentDiaIndex + 1);
    }

    private void createLogItem(int diaId, int until = -1)
    {
        Dialogue[] diaList = diaDic[diaId].dialogues;
        string lastName = null;
        string line = "";
        int extraIndex = 0;

        if (until == -1)
        {
            until = diaList.Length;
        }

        for (int i=0; i < until; i++)
        {
            if (diaList[i].contexts.Equals("")) continue; // 공란은 패스

            if (lastName != diaList[i].name)
            {
                if (lastName != null)
                {
                    GameObject log = Instantiate(logPrefab); //프리팹 생성
                    log.transform.SetParent(Content.transform); //스크롤 뷰 내에 "Content"의 자식들이 스크롤 뷰 리스트로 나타남
                    log.name = "logItem";


                    if (lastName.Equals("")) //나레이션
                    {
                        log.GetComponent<SetLogContent>().setItemInfo(line); 
                    }
                    else //대화
                    {
                        Sprite char_img;
                        float result;
                        if (float.TryParse(lastName, out result)) //캐릭터id면
                        {
                            char_img = porDic[int.Parse(lastName)][0];
                            log.GetComponent<SetLogContent>().setItemInfo(char_img, nameDic[int.Parse(lastName)], line); //정보 넘겨주면 set
                        }
                        else //엑스트라면
                        {
                            char_img = porDic[9999][extraIndex]; //해당 초상화 가져와서
                            log.GetComponent<SetLogContent>().setItemInfo(char_img, lastName, line); //정보 넘겨주면 set
                        }
                    }
                }
                lastName = diaList[i].name;
                line = diaList[i].contexts + "\n";
                extraIndex = diaList[i].portrait_emotion;
            } 
            else
            {
                line += diaList[i].contexts + "\n";
            }
        }

        if (lastName != null)
        {
            GameObject log = Instantiate(logPrefab); //프리팹 생성
            log.transform.SetParent(Content.transform); //스크롤 뷰 내에 "Content"의 자식들이 스크롤 뷰 리스트로 나타남
            log.name = "logItem";


            if (lastName.Equals("")) //나레이션
            {
                log.GetComponent<SetLogContent>().setItemInfo(line);
            }
            else //대화
            {
                Sprite char_img;
                float result;
                if (float.TryParse(lastName, out result)) //캐릭터id면
                {
                    char_img = porDic[int.Parse(lastName)][0];
                    log.GetComponent<SetLogContent>().setItemInfo(char_img, nameDic[int.Parse(lastName)], line); //정보 넘겨주면 set
                }
                else //엑스트라면
                {
                    char_img = porDic[9999][extraIndex]; //해당 초상화 가져와서
                    log.GetComponent<SetLogContent>().setItemInfo(char_img, lastName, line); //정보 넘겨주면 set
                }
            }
        }

    }

    
}
