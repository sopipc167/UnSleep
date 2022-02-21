using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockTutorial : MonoBehaviour
{

    public GameObject[] Canvas0;
    public GameObject[] Canvas1;
    public GameObject[] Canvas2;
    private PuzzleTutorial puzzle;
    private int curr;

    void Start()
    {
        if (Dialogue_Proceeder.instance.CurrentEpiID == 18)
            GetComponent<PuzzleTutorial>().SetOnTutorial(0, 2);
        else if (Dialogue_Proceeder.instance.CurrentEpiID == 23)
            GetComponent<PuzzleTutorial>().SetOnTutorial(2, 4);
        else //보고용. 나중에 지워라
            GetComponent<PuzzleTutorial>().SetOnTutorial(0, 4);

        puzzle = GetComponent<PuzzleTutorial>();
    }

    private void Update()
    {
        curr = puzzle.curr;
        if (!Canvas0[curr].activeSelf)
            ChangeCanvas(curr);
    }

    private void ChangeCanvas(int curr)
    {
        for (int i=0; i < Canvas0.Length; i++)
        {
            Canvas0[i].SetActive(false);
            Canvas1[i].SetActive(false);
            Canvas2[i].SetActive(false);
        }

        Canvas0[curr].SetActive(true);
        Canvas1[curr].SetActive(true);
        Canvas2[curr].SetActive(true);

    }

    public void HowToClockTower()
    {
        if (Dialogue_Proceeder.instance.CurrentEpiID < 19)
            GetComponent<PuzzleTutorial>().SetOnTutorial(0, 2);
        else
            GetComponent<PuzzleTutorial>().SetOnTutorial(0, 4);
    }


}
