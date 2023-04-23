using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialClickException : MonoBehaviour
{

    private void OnDisable()
    {
        ExceptUIClick.isActive = false;
    }
}
