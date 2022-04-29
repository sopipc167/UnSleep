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
    private static int ID;
    private bool startShow;

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
        // 재시작으로 인한 씬이동은 무시한다.
        if (ID == Dialogue_Proceeder.instance.CurrentEpiID) return;
        ID = Dialogue_Proceeder.instance.CurrentEpiID;

        // 만약 새로운 정보가 추가됐다면, 퍼즐 처음부터 정보를 띄운다.
        foreach (var item in info)
        {
            if (Dialogue_Proceeder.instance.CurrentEpiID == item.id)
            {
                currentInfo = item;
                puzzle.SetTutorial(currentInfo.maxInfo, (currentInfo.maxInfo + 2) / 3);
                break;
            }
        }
        puzzle.SetTutorial(currentInfo.maxInfo, (currentInfo.maxInfo + 2) / 3, false);
    }

    public void HowToPuzzle()
    {
        puzzle.SetTutorial(currentInfo.maxInfo, 1);
    }
}
