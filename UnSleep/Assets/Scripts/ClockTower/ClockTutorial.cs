using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockTutorial : MonoBehaviour
{

    private PuzzleTutorial puzzle;

    void Start()
    {

        GetComponent<PuzzleTutorial>().SetOnTutorial(0, 4);

        if (Dialogue_Proceeder.instance.CurrentEpiID == 18)
            GetComponent<PuzzleTutorial>().SetOnTutorial(0, 2); //0, 1 
        else if (Dialogue_Proceeder.instance.CurrentEpiID == 23)
            GetComponent<PuzzleTutorial>().SetOnTutorial(2, 4); //2, 3
        //else //보고용. 나중에 지워라
        //    GetComponent<PuzzleTutorial>().SetOnTutorial(0, 4);

        puzzle = GetComponent<PuzzleTutorial>();
    }

    public void HowToClockTower()
    {
        if (Dialogue_Proceeder.instance.CurrentEpiID < 19)
            GetComponent<PuzzleTutorial>().SetOnTutorial(0, 2);
        else
            GetComponent<PuzzleTutorial>().SetOnTutorial(0, 4);
    }


}
