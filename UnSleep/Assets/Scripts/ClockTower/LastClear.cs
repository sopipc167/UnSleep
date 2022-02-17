using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastClear : MonoBehaviour
{

    public GameObject LastGear;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {

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
