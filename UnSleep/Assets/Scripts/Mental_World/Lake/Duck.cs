using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour
{
    public GameObject duckarea;
    public float movearea_radius = 20f;
    private bool isMoving = false;
    private Vector3 targetVec;
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            targetVec = RamdomPointInSphere(movearea_radius);
        }

        if (Vector3.Distance(transform.position, targetVec) > 0.05f)
        {
            isMoving = true;
            transform.position = Vector3.MoveTowards(transform.position, targetVec, 0.02f);
            transform.LookAt(targetVec);
        }
        else
            isMoving = false;

    }

    public Vector3 RamdomPointInSphere(float radius)
    {
        Vector3 getPoint = Random.onUnitSphere;
        getPoint.y = 0f;

        float r = Random.Range(0f, radius);

        return (getPoint * r) + duckarea.transform.position;
    }
}
