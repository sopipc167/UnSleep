using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Vector3 point;
    public Book_test book;

    void Start()
    {
        
    }

    void Update()
    {
        transform.localPosition = point;
    }

    public void Click()
    {
        point = book.transformPoint(Input.mousePosition);
    }
}
