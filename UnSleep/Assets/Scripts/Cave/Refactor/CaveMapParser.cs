using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveMapParser
{
    
    public Cavern getCavern(TextAsset csv)
    {
        string[] cols = parseCavernData(csv);
        Cavern root = null;
        Cavern prevCavern = null;

        for (int c = 0; c < cols.Length; c++)
        {
            string[] rows = cols[c].Split(new char[] { '\n' });
            List<Cavern> stage = new List<Cavern>();

            for (int r = 0; r < rows.Length; r++)
            {
                string[] roomData = rows[r].Split(new char[] { '#' });
                
                if (roomData[0].Equals("-1")) continue; // 빈 방이면 패스

                if (prevCavern == null)
                {
                    root = new Cavern(roomData);
                    prevCavern = root;
                } else
                {
                    Cavern cavern = new Cavern(roomData);
                    cavern.setPrevCarven(prevCavern);
                    stage.Add(cavern);
                }
            }

            prevCavern.setNextCarven(stage.ToArray());

        }

        return root;
    }

    private string[] parseCavernData(TextAsset csv)
    {
        string[] Selected_Data;
        int rowLength;
        int start;

        if (Dialogue_Proceeder.instance.CurrentEpiID == 7 && Dialogue_Proceeder.instance.AlreadyDone(2014))
        {
            rowLength = 3;
            start = 18;

        }
        else
        {
            switch (Dialogue_Proceeder.instance.CurrentEpiID)
            {
                case 2:
                    rowLength = 4;
                    start = 1;
                    break;
                case 5:
                    rowLength = 4;
                    start = 6;
                    break;
                case 7:
                    rowLength = 6;
                    start = 11;
                    break;
                case 9:
                    rowLength = 5;
                    start = 22;
                    break;
                case 11:
                    rowLength = 4;
                    start = 28;
                    break;
                case 15:
                    rowLength = 5;
                    start = 33;
                    break;
                case 16:
                    rowLength = 4;
                    start = 39;
                    break;
                case 17:
                    rowLength = 3;
                    start = 44;
                    break;
                case 18:
                    rowLength = 1;
                    start = 48;
                    break;
                case 19:
                    rowLength = 2;
                    start = 50;
                    break;
                default:
                    rowLength = 4;
                    start = 1;
                    break;
            }

        }


        string[] tmpstring = csv.text.Split(new char[] { ',' });
        Selected_Data = new string[rowLength];
        for (int i = 0; i < rowLength; i++)
        {
            Selected_Data[i] = tmpstring[start + i];
        }
        return Selected_Data;
    }

}
