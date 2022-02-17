using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBurst : MonoBehaviour
{
    private ParticleSystem ps;
    private Ray ray;
    private Camera mainCam;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        distance = -mainCam.transform.position.z;
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ray = mainCam.ScreenPointToRay(Input.mousePosition);
            transform.position = ray.GetPoint(distance);
            ps.Play();
        }
    }
}
