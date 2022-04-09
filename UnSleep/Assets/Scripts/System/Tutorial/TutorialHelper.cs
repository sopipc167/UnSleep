﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialInfo
{
    public int id;
    public int maxInfo;
}

public class TutorialHelper : MonoBehaviour
{
    [Header("퍼즐의 정보를 제한할 개수")]
    public TutorialInfo[] info;
    private TutorialInfo currentInfo;

    private PuzzleTutorial puzzle;

    private void Awake()
    {
        puzzle = GetComponent<PuzzleTutorial>();
    }

    void Start()
    {
        for (int i = info.Length - 1; i >= 0; --i)
        {
            if (Dialogue_Proceeder.instance.CurrentEpiID >= info[i].id)
            {
                currentInfo = info[i];
                puzzle.SetTutorial(currentInfo.maxInfo, (currentInfo.maxInfo + 2) / 3);
                break;
            }
        }
    }

    public void HowToPuzzle()
    {
        puzzle.SetTutorial(currentInfo.maxInfo, 1);
    }
}
