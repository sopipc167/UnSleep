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
    //public int[] progress; //20개 에피소드의 진행도를 표시. 0: 미완료 1: 완료 
    public int progress; // 가장 마지막에 클리어한 에피소드 (진행도)
}

[Serializable]
public class SystemOption
{
    public float volume_master; //마스터볼륨
    public float volume_bgm; //BGM 볼륨
    public float volume_se; //SE 볼륨
    public bool mute_master; //마스터 뮤트
    public bool mute_bgm; //BGM 뮤트
    public bool mute_se; //SE 뮤트
    public int graphic; //그래픽 설정
    public int resolutionType; //해상도 설정
    public int screenType; //스크린 설정
}

public class SaveDataManager : MonoBehaviour
{
  
    //public int[] epiProgress;
    public int Progress; // 해당 숫자 에피소드를 하면 되는 상황 (Progress - 1 이 곧 진행도)
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
        Progress = LoadEpiProgress();
    }

    public void SaveSystemOption(float vm, float vb, float vs, bool mm, bool mb, bool ms, int g, int r, int s)
    {
        SystemOption systemOption = new SystemOption();

        systemOption.volume_master = vm;
        systemOption.volume_bgm = vb;
        systemOption.volume_se = vs;

        systemOption.mute_master = mm;
        systemOption.mute_bgm = mb;
        systemOption.mute_se = ms;

        systemOption.graphic = g;
        systemOption.resolutionType = r;
        systemOption.screenType = s;

        var textdata = SaveLoad.ObjectToJson(systemOption);
        var AEStextdata = AES256.Encrypt256(textdata, "aes256=32CharA49AScdg5135=48Fk63");

        SaveLoad.CreateJsonFile(Application.dataPath, "SystemOptionData", AEStextdata);

    }

    //생각해봤는데 왜 클래스 만들어놓고 매개변수로 줬죠? 능지수준ㅋㅋㅋㅋ
    public void SaveSystemOption(SystemOption systemOption)
    {
        var textdata = SaveLoad.ObjectToJson(systemOption);
        var AEStextdata = AES256.Encrypt256(textdata, "aes256=32CharA49AScdg5135=48Fk63");

        SaveLoad.CreateJsonFile(Application.dataPath, "SystemOptionData", AEStextdata);

    }


    public YourInfo LoadSystemOption()
    {
        var data = SaveLoad.LoadJsonFileAES<YourInfo>(Application.dataPath, "SystemOptionData", "aes256=32CharA49AScdg5135=48Fk63");
        return data;
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


    public void SaveYourInfo(YourInfo info)
    {

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
            Progress = 0;
            //ep.progress = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        }
        else
        {
            int lastclear = LoadEpiProgress();


            if (epi_num <= lastclear) // 이미 클리어한거 또 클리어한 경우
                return;
            
            ep.progress = epi_num;
            Progress = epi_num;
            
        }



        var textdata = SaveLoad.ObjectToJson(ep);
        var AEStextdata = AES256.Encrypt256(textdata, "aes256=32CharA49AScdg5135=48Fk63");

        SaveLoad.CreateJsonFile(Application.dataPath, "EpiProgressData", AEStextdata);

    }

    public int LoadEpiProgress()
    {
       if (!FileExist()) {
            Debug.Log("No file");
            SaveEpiProgress(0); //초회 세이브 파일 생성. 
        }
        Debug.Log("File exist");
        var data = SaveLoad.LoadJsonFileAES<EpiProgress>(Application.dataPath, "EpiProgressData", "aes256=32CharA49AScdg5135=48Fk63");

        //epiProgress = data.progress;
        return data.progress;
    }

    public bool FileExist()
    {
        return File.Exists(Path.Combine(Application.dataPath, "EpiProgressData.json"));
    }
}
