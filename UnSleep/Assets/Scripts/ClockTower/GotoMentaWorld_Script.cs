using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoMentaWorld_Script : MonoBehaviour
{
    // Start is called before the first frame update
    public void GotoMentalWorld()
    {
        Dialogue_Proceeder.instance.UpdateCurrentDiaIDPlus1();
        SceneManager.LoadScene("Mental_World_Map");

    }
}
