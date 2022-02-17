using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogAnimation : MonoBehaviour
{

    public Animator Log_animator;
    public TextManager manager;
    // Start is called before the first frame update
    void Start()
    {
        //Log_animator = GetComponent<Animator>();
    }


    public void Log_Open()
    {

        Log_animator.SetBool("LogOpen", true);
    }

    public void Log_Close()
    {
        Log_animator.SetBool("LogOpen", false);
      
    }

    public void Log_Close_Event()
    {
        manager.GetComponent<TextManager>().Log_Off2();

    }


}
