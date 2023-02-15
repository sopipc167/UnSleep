using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cavern
{
    public int routeCnt;
    public int talkId;
    public string soundPosition;
    public int soundIndex;
    public float volume;
    public int soundIndex2;
    public float volume2;
    public int objectIndex;
    public bool isSave;

    private Cavern prev = null;
    private Cavern[] next = null;

    public Cavern(string[] info)
    {
        routeCnt = parseInt(info[0]);
        if (info[1].Equals("T"))
        {
            talkId = parseInt(info[2]);
        }
      
        if (info[3].Equals("A"))
        {
            soundPosition = info[4];
            soundIndex = parseInt(info[5]);
            volume = parseFloat(info[6]);

            if (!info[7].Equals(""))
            {
                soundIndex2 = parseInt(info[7]);
                volume2 = parseFloat(info[8]);
            }
        }

        if (!info[9].Equals(""))
        {
            objectIndex = parseInt(info[10]);
        }

        isSave = info[11].Equals("S");
        
    }

    // int로 Parse 가능하면 결과값을, 그렇지 않으면 -1을 반환 
    // Todo: ParseUtil로 빼기
    private int parseInt(string target)
    {
        int result;
        if (int.TryParse(target, out result)) 
            return result; 
        else 
            return -1;
    }

    private float parseFloat(string target)
    {
        float result;
        if (float.TryParse(target, out result))
            return result;
        else
            return -1f;
    }


    public void setPrevCarven(Cavern cavern)
    {
        prev = cavern;
    }

    public void setNextCarven(Cavern[] cavernList)
    {
        next = cavernList;
    }

    public Cavern back()
    {
        return prev;
    }

    public Cavern proceed(int idx)
    {
        return next[idx];
    }
}
