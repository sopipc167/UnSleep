using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MapGenerator : MonoBehaviour
{
    public int Epi_num;
    //public TextAsset[] MapDatas;
    //public int Map_End_Row;
    public MapArray[] Row;

    public Dictionary<string, GameObject> MapDic;

    public GameObject[] CavePrepabs;
    public GameObject CAVE;

    public string[] checkstring;

    //<------------------>
   
    public TextAsset MapTable;
    public int RowLength;
    public int parse_start;



    void Awake()
    {
        Extract_Map();
        MapDic["00"].SetActive(true);
    }



    public string[] Load_MapTable_Data()
    {
        string[] Selected_Data;
        
        if (Epi_num == 9)
        {
            RowLength = 4;
            parse_start = 1;
        }
        else if (Epi_num == 19)
        {
            RowLength = 4;
            parse_start = 6;
        }
        else if (Epi_num == 20)
        {
            RowLength = 6;
            parse_start = 11;
        }
        else if (Epi_num == 21)
        {
            RowLength = 3;
            parse_start = 18;
        }
        else if (Epi_num == 24)
        {
            RowLength = 5;
            parse_start = 22;
        }
        else if (Epi_num == 27)
        {
            RowLength = 4;
            parse_start = 28;
        }
        else if (Epi_num == 50)
        {
            RowLength = 5;
            parse_start = 33;
        }
        else if (Epi_num == 56)
        {
            RowLength = 4;
            parse_start = 39;
        }
        else if (Epi_num == 65)
        {
            RowLength = 3;
            parse_start = 44;
        }
        else if (Epi_num == 70)
        {
            RowLength = 1;
            parse_start = 48;
        }
        else if (Epi_num == 80)
        {
            RowLength = 2;
            parse_start = 50;
        }

        string[] tmpstring = MapTable.text.Split(new char[] { '\n' });
        Selected_Data = new string[RowLength];
        for (int i=0; i < RowLength; i++)
        {
            Selected_Data[i] = tmpstring[parse_start + i];
        }
        return Selected_Data;
    }

    public void Extract_Map()
    {
        string[] row = Load_MapTable_Data();
        string[] col_temp = row[0].Split(new char[] { ',' });
        int col_count = col_temp.Length;
        float result;

        Row = new MapArray[RowLength];
        MapDic = new Dictionary<string, GameObject>();

        checkstring = col_temp;
        for (int i=0; i < RowLength; i++)
        {
            string[] col_temp_ = row[i].Split(new char[] { ',' });
            

            Row[i] = new MapArray();
            Row[i].Col = new Map[col_count];

            for (int j = 0; j < col_count; j++)
            {
                string[] col_element = col_temp_[j].Split(new char[] { '#' });

                if (col_element.Equals(""))
                    continue;



                Row[i].Col[j] = new Map();
                Row[i].Col[j].mapcode = i.ToString() + j.ToString();

                

                if (float.TryParse(col_element[0], out result)) //숫자면
                {
                    int r = int.Parse(col_element[0]);
                    Row[i].Col[j].route = r;
                    if (r != -1)
                    {
                        if (r >= 0 && r < 4)
                        {
                            GameObject cur_cave = MonoBehaviour.Instantiate(CavePrepabs[r]); //프리팹 생성
                            cur_cave.name = Row[i].Col[j].mapcode;
                            cur_cave.transform.SetParent(CAVE.transform);
                            cur_cave.transform.localPosition = Vector3.zero;

                            MapDic[Row[i].Col[j].mapcode] = cur_cave;
                            cur_cave.SetActive(false);
                        }
                        else if (r==999)
                        {
                            GameObject cur_cave = MonoBehaviour.Instantiate(CavePrepabs[4]); //프리팹 생성
                            cur_cave.name = "destination"+ Row[i].Col[j].mapcode;
                            cur_cave.transform.SetParent(CAVE.transform);
                            cur_cave.transform.localPosition = Vector3.zero;

                            MapDic[Row[i].Col[j].mapcode] = cur_cave;
                            cur_cave.SetActive(false);

                        }
                    }
                }


                
                
                if (col_element[1].Equals("T"))
                {
                    Row[i].Col[j].isTalk = true;
                    Row[i].Col[j].talk_id = int.Parse(col_element[2]);
                }

                if (col_element[3].Equals("A"))
                {
                    Row[i].Col[j].isAudio = true;
                    Row[i].Col[j].sound_position = col_element[4];
                    Row[i].Col[j].sound_index = int.Parse(col_element[5]);
                    //if (float.TryParse(col_element[3], out result))
                    Row[i].Col[j].volume = float.Parse(col_element[6]);

                    if (!col_element[7].Equals(""))
                    {
                        Row[i].Col[j].sound_index_2 = int.Parse(col_element[7]);
                        Row[i].Col[j].volume_2 = float.Parse(col_element[8]);

                    }

                }

                if (!col_element[9].Equals(""))
                {
                    Row[i].Col[j].isObject = true;
                    Row[i].Col[j].object_index = int.Parse(col_element[10]);

                }



                if (col_element[11].Equals("S"))
                {
                    Row[i].Col[j].isSave = true;
                }


            }
        }

    }



    /*
         public void Extract_Map()
    {
        //string[] row = Load_Cave_Csv().text.Split(new char[] { '\n' });
        string[] row = Load_MapTable_Data();
        string[] col_temp = row[0].Split(new char[] { ',' });
        int col_count = col_temp.Length;
        float result;

        Row = new MapArray[Map_End_Row];
        MapDic = new Dictionary<string, GameObject>();


        for (int i=0; i < Map_End_Row; i++)
        {
            string[] col_temp_ = row[i].Split(new char[] { ',' });
            

            Row[i] = new MapArray();
            Row[i].Col = new Map[col_count];

            for (int j = 0; j < col_count; j++)
            {
                Row[i].Col[j] = new Map();
                Row[i].Col[j].mapcode = i.ToString() + j.ToString();

                string[] col_element = col_temp_[j].Split(new char[] { '#' });
                //checkstring = col_temp_[j].Split(new char[] { '#' });
                //checkstring = col_element;

                if (float.TryParse(col_element[0], out result)) //숫자면
                {
                    int r = int.Parse(col_element[0]);
                    Row[i].Col[j].route = r;
                    if (r != -1)
                    {
                        if (r >= 0 && r < 4)
                        {
                            GameObject cur_cave = MonoBehaviour.Instantiate(CavePrepabs[r]); //프리팹 생성
                            cur_cave.name = Row[i].Col[j].mapcode;
                            cur_cave.transform.SetParent(CAVE.transform);
                            cur_cave.transform.localPosition = Vector3.zero;

                            MapDic[Row[i].Col[j].mapcode] = cur_cave;
                            cur_cave.SetActive(false);
                        }
                        else if (r==999)
                        {
                            GameObject cur_cave = MonoBehaviour.Instantiate(CavePrepabs[4]); //프리팹 생성
                            cur_cave.name = "destination"+ Row[i].Col[j].mapcode;
                            cur_cave.transform.SetParent(CAVE.transform);
                            cur_cave.transform.localPosition = Vector3.zero;

                            MapDic[Row[i].Col[j].mapcode] = cur_cave;
                            cur_cave.SetActive(false);

                        }
                    }
                }


                if (col_element[1].Equals("T"))
                {
                    Row[i].Col[j].isTalk = true;
                    Row[i].Col[j].talk_id = int.Parse(col_element[2]);
                }

                if (col_element[3].Equals("A"))
                {
                    Row[i].Col[j].isAudio = true;
                    Row[i].Col[j].sound_position = col_element[4];
                    Row[i].Col[j].sound_index = int.Parse(col_element[5]);
                    //if (float.TryParse(col_element[3], out result))
                    Row[i].Col[j].volume = float.Parse(col_element[6]);

                    if (!col_element[7].Equals(""))
                    {
                        Row[i].Col[j].sound_index_2 = int.Parse(col_element[7]);
                        Row[i].Col[j].volume_2 = float.Parse(col_element[8]);

                    }

                }

                if (!col_element[9].Equals(""))
                {
                    Row[i].Col[j].isObject = true;
                    Row[i].Col[j].object_index = int.Parse(col_element[10]);

                }



                if (col_element[11].Equals("S"))
                {
                    Row[i].Col[j].isSave = true;
                }


            }
        }

    }

     */
}
