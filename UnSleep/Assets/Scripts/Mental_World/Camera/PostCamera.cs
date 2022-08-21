using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostCamera : MonoBehaviour
{
    private Camera mainCam;
    private Camera postCam;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main.GetComponent<Camera>();
        postCam = GetComponent<Camera>();
    }


    private void LateUpdate()
    {
        postCam.transform.position = mainCam.transform.position;
        postCam.transform.rotation = mainCam.transform.rotation;

    }
}
