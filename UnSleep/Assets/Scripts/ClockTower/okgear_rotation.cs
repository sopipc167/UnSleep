using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class okgear_rotation : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Okgear_img;

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
        Okgear_img.transform.Rotate(new Vector3(0f, 0f, -200f) * Time.deltaTime);

    }
}
