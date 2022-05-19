using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//파싱하여 딕셔너리에 모두 저장, 에피소드 시작 시 Awake()로 한방에 파싱  
public class DialogueParser : MonoBehaviour
{
    private TextAsset csvData; //csv파일, 일단은 인스펙터에서 넣기
    public static bool ParsingisFinish = false; //나중에 파싱 로딩할 때 끊김 방지용
    private string CharIdCell;

    //7세
    public string con;

    private TextAsset LoadCSV()
    {
        int EpiId = Dialogue_Proceeder.instance.CurrentEpiID;
        TextAsset csv = Resources.Load<TextAsset>("epi_csv/epi_" + EpiId.ToString());
        return csv;
    }
     
    public DialogueEvent[] Parse_Dialogue() //start에서 finish까지의 라인을 파싱 
    {
        csvData = LoadCSV();
        List<DialogueEvent> diaEList = new List<DialogueEvent>(); //마지막에 return할 리스트, 각 요소는 대화 묶음
        
        string[] data = csvData.text.Split(new char[] { '\n' }); //개행문자 단위로 자름 (가로 한 줄)
        string[] HeadRow = data[0].Split(new char[] { ',' });
        CharIdCell = HeadRow[1];

        for (int i = 2; i < data.Length-1; i++) //1부터 마지막 줄 까지의 라인을 파싱 
        {
            string[] row = data[i].Split(new char[] { ',' }); //data를 , 단위로 자름 (한 칸 씩)

            DialogueEvent diaE = new DialogueEvent(); //DialogueEvent 변수 하나 만들어서 정보 저장 후 리스트에 추가

            diaE.SceneNum = int.Parse(row[0]); //대화 이벤트 이름

            diaE.Place = row[1]; //내용이 있으면 장소로. 추후 맵 오브젝트에 투명 오브젝트를 배치하여 각 장소의 위치를 지정하여 스폰

            //if (row[1].Equals("")) //장소 공란 => 스토리 모드 
            //    diaE.isStory = true; 

            if (!row[2].Equals(""))
                diaE.DiaKey = int.Parse(row[2]); //대화 묶음. 파싱하고 나면 모든 정보는 string이므로 int로 형변환

            //대화 발생 조건
            if (row[7].Equals("")) //공란일 경우 조건 없음. 정수 0으로 표현
                diaE.Condition = new int[] { 0 };
            else if (!row[7].Contains("|")) //조건이 1개인 경우
            {
                diaE.Condition = new int[1];
                diaE.Condition[0] = int.Parse(row[7]);
            }
            else //조건이 여러개일 경우   string을 | 단위로 끊어서 int 배열로 저장   ex) 1801|1802|1803 -> {1801, 1802, 1803}
            {
                string[] conditions = row[7].Split(new char[] { '|' });
                diaE.Condition = new int[conditions.Length];

                for (int j=0; j < conditions.Length; j++)
                {
                    diaE.Condition[j] = int.Parse(conditions[j]);
                }

            }


            if (!row[13].Equals("")) 
                diaE.BGM = row[13]; //배경음

            List<Dialogue> dialogueList = new List<Dialogue>(); //파싱된 대사를 임시 저장할 리스트
           
            for(;i<= data.Length; i++) //### 대화 묶음 대사 리스트 생성 ###
            {
                string[] cur_row = data[i].Split(new char[] { ',' }); //data를 , 단위로 자름
                string[] next_row = data[i + 1].Split(new char[] { ',' }); //대화 묶음 id 구분 위해 다음 data가지고 옴
                                                                           //*****주의 : 테이블 가장 마지막 줄에 - 등 아무거나 적어서 공란이 아니도록 **********

                Dialogue dia = new Dialogue(); //Dialogue 생성


                dia.name = cur_row[3]; //발화자
                dia.contexts = cur_row[4].Replace("`",","); //대사 속 `를 , 로 바꾸고 저장

                if (cur_row[9].CompareTo("0")==1|| cur_row[9].CompareTo("1") == 1 || cur_row[9].CompareTo("2") == 1)
                    dia.portrait_position = int.Parse(cur_row[9]); //발화자 위치 0:독백 1:좌 2:우


                if (!cur_row[10].Equals("")) //레이아웃 변화
                    dia.layoutchange = int.Parse(cur_row[10]); 

                if (!cur_row[8].Equals(""))
                    dia.portrait_emotion = int.Parse(cur_row[8]); //초상화 표정

                if (string.Compare(cur_row[5], "1") == 0) //선택지 대사 여부
                    dia.isSelect = true;
                else
                    dia.isSelect = false;

                if (dia.isSelect) //선택지 대사면? 선택지 관련 정보 넣기 
                {
                    dia.nextDiaKey = int.Parse(cur_row[6]);
                }

                if (!cur_row[11].Equals("")) //공란이 아니면 
                    dia.BG = cur_row[11]; //배경

                if (!cur_row[12].Equals(""))//공란이 아니면 
                    dia.Content = cur_row[12]; //상호작용명


                if (!cur_row[14].Equals(""))
                    dia.SE = cur_row[14]; //효과음

                dialogueList.Add(dia); //대사 한줄을 리스트에 추가 

                if (next_row[2].Equals(""))
                {
                    i++;
                    break;
                }
                else if (!cur_row[2].Equals(next_row[2])) //대화묶음의 마지막 대사면 break. (ex. 1801 -> 1802 로 넘어가는 시점)
                {
                    break;
                }

            }



            diaE.dialogues = dialogueList.ToArray(); //대사 리스트를 배열로 만들어서 저장
            diaE.dialogues_size = diaE.dialogues.Length; 

            diaEList.Add(diaE); //한 대화 묶음을 리스트에 추가
        }

        return diaEList.ToArray(); //완성된 대화 묶음 리스트를 반환
    }

    


   public int[] GetCharId()
    {
        
        string[] charId = CharIdCell.Split(new char[] { '|' });
        int[] int_charId = new int[charId.Length];

        for (int i= 0; i < charId.Length; i++)
        {
            int_charId[i] = int.Parse(charId[i]);
        }

        return int_charId;
    }

    /*
    부가 설명
    
    반환된 대화 묶음 리스트는 TextManager의 Awake에서 DiaDic이라는 대화 묶음 딕셔너리로 가공됩니다
    원래는 여기서 가공해서 딕셔너리로 넘겼었는데, 뭔가 오류가 계속 나서.. 여기서는 배열로 리턴해줍니다
    DiaDic은 여기의 diaEList의 대화묶음을 (1801, diaEList[0]) (1802, diaEList[1]) 이런식으로
    대화 묶음 id로 해당 대화 묶음을 찾을 수 있도록 합니다.

     */


}
