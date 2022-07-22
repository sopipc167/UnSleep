using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkAnimation : MonoBehaviour
{
    private Animator ani;
    public GameObject upper;
    public GameObject lower;
    public bool isSeven_Close;
    public Image Fade;

    void Awake()
    {
        ani = GetComponent<Animator>();
        
    }



    public void BlinkOpen()
    {
        Blink_Enable();
        upper.GetComponent<Image>().color = new Color(0, 0, 0, 1f);
        lower.GetComponent<Image>().color = new Color(0, 0, 0, 1f);
        ani.SetTrigger("BlinkOpen");
        Invoke("Blink_Disable", 3f);

    }

    public void BlinkClose()
    {
        upper.GetComponent<Image>().color = new Color(0, 0, 0, 0f);
        lower.GetComponent<Image>().color = new Color(0, 0, 0, 0f);

        Blink_Enable();
        ani.SetTrigger("BlinkClose");
        if (isSeven_Close)
        {
            Invoke("Blink_Disable", 3f);
        }
    }

    public void Blink_Disable()
    {
        upper.SetActive(false);
        lower.SetActive(false);
        if (isSeven_Close)
        {
            Color tmp = Fade.color;
            tmp.a = 255;
            Fade.color = tmp;
            isSeven_Close = false;
        }
    }

    public void Blink_Enable()
    {
        upper.SetActive(true);
        lower.SetActive(true);
    }
}
