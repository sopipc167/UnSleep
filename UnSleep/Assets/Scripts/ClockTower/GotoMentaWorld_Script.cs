using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoMentaWorld_Script : MonoBehaviour
{
    // Start is called before the first frame update
    public void GoToNext()
    {

    }

    public void GotoMentalWorld()
    {
        Dialogue_Proceeder.instance.UpdateCurrentDiaIDPlus1();
        SceneChanger.ChangeScene(SceneType.Mental);
    }
}
