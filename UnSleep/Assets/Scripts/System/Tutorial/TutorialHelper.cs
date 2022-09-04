using System.Collections;
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
    private static int ID = -1;

    [Header("퍼즐의 정보를 제한할 개수")]
    public TutorialInfo[] info;
    private TutorialInfo currentInfo;
    private int showInfo = 0;

    private PuzzleTutorial puzzle;

    private void Awake()
    {
        puzzle = GetComponent<PuzzleTutorial>();
    }

    void Start()
    {
        // 재시작으로 인한 씬이동은 무시한다.
        if (ID == Dialogue_Proceeder.instance.CurrentEpiID) return;
        ID = Dialogue_Proceeder.instance.CurrentEpiID;

        // 만약 새로운 정보가 추가됐다면, 퍼즐 처음부터 정보를 띄운다.
        int idx = 0;
        foreach (var item in info)
        {
            if (Dialogue_Proceeder.instance.CurrentEpiID == item.id)
            {
                currentInfo = item;
                int newPage = (currentInfo.maxInfo + 2) / 3;
                int curPage = (showInfo + 2) / 3;
                if (newPage != curPage)
                {
                    if (showInfo % 3 == 0)
                    {
                        puzzle.SetTutorial(currentInfo.maxInfo, curPage + 1);
                    }
                    else
                    {
                        puzzle.SetTutorial(currentInfo.maxInfo, curPage);
                    }
                }
                else
                {
                    puzzle.SetTutorial(currentInfo.maxInfo, curPage);
                }
                showInfo = currentInfo.maxInfo;
                break;
            }
            ++idx;
        }
    }

    public void HowToPuzzle()
    {
        puzzle.SetTutorial(currentInfo.maxInfo, 1);
    }
}
