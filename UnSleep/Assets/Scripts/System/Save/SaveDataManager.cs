using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[Serializable]
public class YourInfo
{
    public string name; //이름
    public int person; //어떤 사람? 0: 상냥 1: 성실 2: 열정
    public int season; //좋아하는 계절? 0: 봄, 1:여름 , 2:가을 , 3:겨울
    public int message; //듣고 싶은 말? 0: 고마워,1: 사랑해, 2:고생했어
    public string dream; //꿈
}

[Serializable]
public class EpiProgress
{
    public int[] progress; //20개 에피소드의 진행도를 표시. 0: 미완료 1: 완료 
}

[Serializable]
public class SystemOption
{
    //시스템 옵션
}

public class SaveDataManager : MonoBehaviour
{
    private EpiProgress EP;
    public int[] epiProgress;
    private static SaveDataManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static SaveDataManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    private void Start()
    {

        //타이틀 씬으로 옮기기
        SaveEpiProgress(0);
        EP = LoadEpiProgress();
        epiProgress = EP.progress;
    }

    public void SaveYourInfo(string _name, int _person, int _season, int _message, string _dream)
    {
        YourInfo info = new YourInfo();
        info.name = _name;
        info.person = _person;
        info.season = _season;
        info.message = _message;
        info.dream = _dream;

        var textdata = SaveLoad.ObjectToJson(info);
        var AEStextdata = AES256.Encrypt256(textdata, "aes256=32CharA49AScdg5135=48Fk63");

        SaveLoad.CreateJsonFile(Application.dataPath, "YourInfoData", AEStextdata);
    }

    public YourInfo LoadYourInfo()
    {
        var data = SaveLoad.LoadJsonFileAES<YourInfo>(Application.dataPath, "YourInfoData", "aes256=32CharA49AScdg5135=48Fk63");
        return data;
    }

    public void SaveEpiProgress(int epi_num) //에피소드 완수 시 호출하여 진행사항 저장. 
    {
        EpiProgress ep = new EpiProgress();

        if (epi_num == 0) //세이브 데이터가 없을 시 0. 게임 최초 실행 시 실행. 세이브 데이터 초기화 용도로도 가능
        {
            ep.progress = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        }
        else
        {
            ep = LoadEpiProgress();
        }


        switch (epi_num)
        {
            case 6:
                ep.progress[0] = 1;
                break;
            case 7:
                ep.progress[1] = 1;
                break;
            case 9:
                ep.progress[2] = 1;
                break;
            case 15:
                ep.progress[3] = 1;
                break;
            case 18:
                ep.progress[4] = 1;
                break;
            case 19:
                ep.progress[5] = 1;
                break;
            case 20:
                ep.progress[6] = 1;
                break;
            case 21:
                ep.progress[7] = 1;
                break;
            case 23:
                ep.progress[8] = 1;
                break;
            case 24:
                ep.progress[9] = 1;
                break;
            case 27:
                ep.progress[10] = 1;
                break;
            case 28:
                ep.progress[11] = 1;
                break;
            case 31:
                ep.progress[12] = 1;
                break;
            case 32:
                ep.progress[13] = 1;
                break;
            case 45:
                ep.progress[14] = 1;
                break;
            case 50:
                ep.progress[15] = 1;
                break;
            case 56:
                ep.progress[16] = 1;
                break;
            case 65:
                ep.progress[17] = 1;
                break;
            case 70:
                ep.progress[18] = 1;
                break;
            case 80:
                ep.progress[19] = 1;
                break;

        }

        var textdata = SaveLoad.ObjectToJson(ep);
        var AEStextdata = AES256.Encrypt256(textdata, "aes256=32CharA49AScdg5135=48Fk63");

        SaveLoad.CreateJsonFile(Application.dataPath, "EpiProgressData", AEStextdata);

    }

    public EpiProgress LoadEpiProgress()
    {
       if (!File.Exists(Path.Combine(Application.dataPath, "EpiProgressData.json"))) {
            Debug.Log("No file");
            SaveEpiProgress(0); //초회 세이브 파일 생성. 
        }
        Debug.Log("File exist");
        var data = SaveLoad.LoadJsonFileAES<EpiProgress>(Application.dataPath, "EpiProgressData", "aes256=32CharA49AScdg5135=48Fk63");
        return data;
        
    }

}
