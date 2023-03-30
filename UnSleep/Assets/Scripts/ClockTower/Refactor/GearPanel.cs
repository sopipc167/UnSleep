using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GearPanel : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.GetComponent<WCogWheel>().inactive();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.GetComponent<WCogWheel>().idle();
    }

}
