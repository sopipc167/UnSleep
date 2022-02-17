using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portrait : MonoBehaviour
{

    private Sprite[] Portraits;
    
    //얘네 켜서 인스펙터로 점검
    //public Sprite[] DebugCheck;
    //public List<Sprite> DebugCheckList;





    //읽을 때 아예 에피소드에 맞는 연령대의 초상화만 불러오도록
    public Dictionary<int, Sprite[]> GetPortraitDic(int[] cha_id, int epi_id) //캐릭터id, 에피소드(18세면 18, 20세면 20)
    {
        Dictionary<int, Sprite[]> PorDic = new Dictionary<int, Sprite[]>(); //리턴 값으로 넘겨줄 딕셔너리

        for (int i =0; i < cha_id.Length; i++) //cha_id 배열 탐색
        {
            Portraits = Resources.LoadAll<Sprite>("Standing_Image/" + cha_id[i].ToString()); //초상화 폴더명은 캐릭터id 
            
            List<Sprite> Por_list = new List<Sprite>(); 
            int por_idx = get_por_idx(cha_id[i], epi_id); //에피소드 당 필요한 초상화의 인덱스 가져오기

            for (int j = 0; j < emotion_cnt(cha_id[i]) * 2; j++, por_idx++) //한 초상화를 좌우 묶음으로
            {
                Por_list.Add(Portraits[por_idx]); //필요한 초상화를 Por_list에 넣기 (ex. 교복 도문이 좌10 우10 총 20개를 하나의 리스트에 넣기)
            }

            PorDic.Add(cha_id[i], Por_list.ToArray()); //(캐릭터 id, 초상화 배열)을 딕셔너리에 추가
        }

        //엑스트라: 얼마 없으니 그냥 다 넣기
        Portraits = Resources.LoadAll<Sprite>("Standing_Image/9999");
        List<Sprite> extra_list = new List<Sprite>();
        for (int j = 0; j <Portraits.Length; j++) //한 초상화를 좌우 묶음으로
        {
            extra_list.Add(Portraits[j]);
        }

        PorDic.Add(9999, extra_list.ToArray()); //엑스트라는 9999에 모두 저장, 엑스트라는 초상화id(표정)으로 그림 고르기 


        return PorDic; //반환
    }


    public int emotion_cnt(int cha_id)
    {
        if (cha_id == 1002 || cha_id == 1003) //어머니 아버지
            return 3;
        else if (cha_id >= 1000 && cha_id <= 1007) //잠재우미 도문 재준 장현 새나 이비
            return 10;
        else
            return 1; //엑스트라
    }

    public int get_por_idx(int cha_id, int epi_id) //에피소드별 필요한 초상화가 시작되는 인덱스를 찾아주는 함수
    {
        if (cha_id==1000)
        {
            return 0;
        }
        else if (cha_id == 1001) //도문 
        {
            if (epi_id <= 6)
                return 0;
            else if (epi_id <= 12)
                return 20;
            else if (epi_id <= 19)
                return 40;
            else if (epi_id <= 25)
                return 60;
            else if (epi_id <= 32) //도문이 턱시도는 예외로 처리하기
                return 80;
            else if (epi_id <= 56)
                return 100;
            else if (epi_id <= 67)
                return 120;
            else
                return -1;
        }
        else if (cha_id == 1004 || cha_id == 1005) //재준 장현
        {
            if (epi_id <= 12)
                return 0;
            else if (epi_id <= 19)
                return 20;
            else if (epi_id <= 25)
                return 40;
            else if (epi_id <= 32)
                return 60;
            else if (epi_id <= 56) //도문이 턱시도는 예외로 처리하기
                return 80;
            else if (epi_id <= 67)
                return 100;
            else
                return -1;
        }
        else if (cha_id == 1002 || cha_id == 1003) //부모님
        {
            if (epi_id <= 12)
                return 0;
            else if (epi_id <= 27)
                return 6;
            else
                return 12;
        }
        else if (cha_id == 1006) //새나
        {
            if (epi_id <= 24)
                return 0;
            else if (epi_id <= 27)
                return 20;
            else if (epi_id <= 32)
                return 40;
            else if (epi_id <= 56)
                return 60;
            else
                return 80;
        }
        else if (cha_id == 1007) //이비
        {
            if (epi_id <= 45)
                return 0; //애기
            else if (epi_id <= 50)
                return 20; //교복
            else if (epi_id <= 56)
                return 40; //성인(대학생)
            else
                return 60; //직장인

        }
        else
            return -1;
    }

}

