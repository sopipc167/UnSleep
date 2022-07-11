using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class DiaryText
{
    public int epi_id; //에피소드 id
    public string epi_title; //에피소드 제목
    public string epi_intro; //에피소드 소개
    public string afterstory; //후일담
    public CharacIntro[] characs; //등장인물 설명
}

[System.Serializable]
public class CharacIntro //등장인물 설명
{
    public string name; //이름
    public string intro; //설명
}


public class DiaryTextParser : MonoBehaviour
{

    public TextAsset diarytable; //일기장 텍스트 csv 파일 참조
    //public List<DiaryText> diaryTexts; //여기에 정보가 담겨있음
     

    public List<DiaryText> ParseDiaryText()
    {
        List<DiaryText> diaryTexts = new List<DiaryText>();

        string[] row = diarytable.text.Split(new char[] { '\n' }); //개행문자 단위로 싹둑

        for (int i=1; i < 21; i++)
        {
            
            string[] line = row[i].Split(new char[] { ',' });
            DiaryText dt = new DiaryText();

            dt.epi_id = int.Parse(line[0]);
            dt.epi_title = line[1];
            dt.epi_intro = line[2];
            dt.afterstory = line[3];

            List<CharacIntro> CIlist = new List<CharacIntro>();
            for (int j = 0; j < line.Length; j+=2)
            {


     

                if (j + 4 >= line.Length || line[4 + j].Equals("")) 
                    break;



                CharacIntro ci = new CharacIntro();
                ci.name = line[4 + j];
                ci.intro = line[5 + j];

                CIlist.Add(ci);
            }


            dt.characs = CIlist.ToArray();

            diaryTexts.Add(dt);
        }

        return diaryTexts;
    }


}


