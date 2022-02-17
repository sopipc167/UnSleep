using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cave_Animation : MonoBehaviour
{
    Animator animator;
    GameObject other_speaker;
    CaveOtherSound caveOtherSound;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        caveOtherSound = GameObject.Find("other_speaker").GetComponent<CaveOtherSound>();
    }


    public void Ani_Proceed()
    {
        animator.SetBool("Proceed", true);
        caveOtherSound.Walk_Sound();
    }

    public void Ani_Back()
    {
        animator.SetBool("Back", true);
        caveOtherSound.Walk_Sound();
    }

    public void Ani_Reset()
    {
        animator.SetBool("Proceed", false);
        animator.SetBool("Back", false);
    }
}
