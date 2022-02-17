using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoLine : MonoBehaviour
{
    internal EraseLine erase;
    private LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (erase.isErasing)
        {
            int size = line.positionCount;
            if (size == 1) Destroy(gameObject);

            for (int i = 0; i < size; i++)
            {
                if (Vector3.Distance(erase.mousePos, line.GetPosition(i)) <= erase.radius)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
