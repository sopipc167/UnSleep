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
    //임시
    [Header("임시 - 테스트용")]
    public bool showAtStart;
    public int maxSize;

    [Header("퍼즐의 정보를 제한할 개수")]
    public TutorialInfo[] info;
    private TutorialInfo currentInfo;

    private PuzzleTutorial puzzle;

    private void Awake()
    {
        puzzle = GetComponent<PuzzleTutorial>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            puzzle.SetTutorial(maxSize, 1);
        }
    }

    void Start()
    {
        if (showAtStart)
        {
            puzzle.SetTutorial(maxSize, 1);
            return;
        }

        foreach (var item in info)
        {
            if (Dialogue_Proceeder.instance.CurrentEpiID == item.id)
            {
                currentInfo = item;
                puzzle.SetTutorial(currentInfo.maxInfo, (currentInfo.maxInfo - 1) / 3);
                break;
            }
        }
    }

    public void HowToPuzzle()
    {
        puzzle.SetTutorial(currentInfo.maxInfo, 1);
    }
}
