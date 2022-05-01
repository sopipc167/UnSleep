using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastClear : MonoBehaviour
{

    public GameObject LastGear;
    public TextManager textManager;

    private void Start()
    {
        textManager.Set_Dialogue_System();
    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (LastGear.GetComponent<Gear_Drag_new>().Stop)
            return;

        //LastGear.transform.parent.transform.position = transform.position;
        StartCoroutine("MoveTo");
        LastGear.GetComponent<Gear_new>().LastGear = false;
        LastGear.GetComponent<Gear_new>().initial_speed = 1200f;
        LastGear.GetComponent<Gear_Drag_new>().Stop = true;

        Dialogue_Proceeder.instance.AddCompleteCondition(34);
        textManager.Set_Dialogue_Goodbye();

    }

    IEnumerator MoveTo()
    {
        while (Vector3.Distance(LastGear.transform.parent.transform.position, transform.position) > 0.01f)
        {
            LastGear.transform.parent.transform.position = Vector3.MoveTowards(LastGear.transform.parent.transform.position, transform.position, 0.5f);
            yield return null;
        }

    }
}
