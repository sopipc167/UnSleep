using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveMapParser: MonoBehaviour
{
    
    private string[][][] caveMapInfo;
    private ParseUtil parseUtil = ParseUtil.Instance();



    public Cavern getRootCavern(TextAsset caveCsv)
    {
        caveMapInfo = parseCavernData(caveCsv);
        Cavern root = new Cavern(caveMapInfo[0][0]);
        Cavern[] child = composeCarvens(0, 0, root.routeCnt);
        root.setNextCarven(child);

        return root;
    }

    private Cavern[] composeCarvens(int stage, int start, int count)
    {
        List<Cavern> caverns = new List<Cavern>();
        int accumulate = 0;


        for (int s = start; s < start + count; s++)
        {
            string[] cell = caveMapInfo[stage + 1][s];
            int routeCount = parseUtil.parseInt(cell[0]);
           

            if (routeCount >= 0)
            {
                Cavern cavern = new Cavern(cell);

                if (routeCount > 0 && routeCount < 999)
                    cavern.setNextCarven(composeCarvens(stage + 1, accumulate, cavern.routeCnt));

                caverns.Add(cavern);
                accumulate += cavern.routeCnt;
            }
        }

        return caverns.ToArray();
    }


    private string[][][] parseCavernData(TextAsset csv)
    {
        List<string> rowList = new List<string>();
        
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

        string[] rows = csv.text.Split(new char[] { '\n' });
        
        for (int i = 0; i < rowLength; i++)
        {
            rowList.Add(rows[start + i]);
        }

        return parseStageData(rowList);
    }

    private string[][][] parseStageData(List<string> rowList)
    {
        List<string[][]> carvenDataList = new List<string[][]>();
        List<string[]> splitedRowList = new List<string[]>();
        int depth = -1;
        
        foreach (string row in rowList)
        {
            splitedRowList.Add(row.Split(new char[] { ',' }));
            if (depth < 0) depth = row.Split(new char[] { ',' }).Length;
        }
 
        for (int i = 0; i < depth; i++)
        {
            List<string[]> column = new List<string[]>();

            for (int r = 0; r < rowList.Count; r++)
            {
                column.Add(splitedRowList[r][i].Split(new char[] { '#' }));
            }

            carvenDataList.Add(column.ToArray());
        }

        return carvenDataList.ToArray();
    }

}
