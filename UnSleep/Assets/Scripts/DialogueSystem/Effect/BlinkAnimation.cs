using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkAnimation : MonoBehaviour
{
    private Animator ani;
    public GameObject upper;
    public GameObject lower;

    void Awake()
    {
        ani = GetComponent<Animator>();
        
    }



    public void BlinkOpen()
    {
        Blink_Enable();
        ani.SetTrigger("BlinkOpen");
        Invoke("Blink_Disable", 3f);

    }

    public void BlinkClose()
    {
        Blink_Enable();
        ani.SetTrigger("BlinkClose");
        Invoke("Blink_Disable", 3f);
    }

    public void Blink_Disable()
    {
        upper.SetActive(false);
        lower.SetActive(false);
    }

    public void Blink_Enable()
    {
        upper.SetActive(true);
        lower.SetActive(true);
    }
}
