using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour
{
    public Transform player;
    public GameObject duckarea;
    public float movearea_radius = 20f;
    private bool isMoving = false;
    private Vector3 targetVec;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            targetVec = RamdomPointInSphere(movearea_radius);
        }

        Vector3 peerVec = player.position - transform.position;
        peerVec.y = 0f;
        Vector3 resultVec = targetVec - transform.position;
        resultVec.y = 0;
        if (Vector3.Cross(peerVec, resultVec).y > 0f)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }

        if (Vector3.Distance(transform.position, targetVec) > 0.05f)
        {
            isMoving = true;
            transform.position = Vector3.MoveTowards(transform.position, targetVec, 0.02f);
            //transform.LookAt(targetVec);
        }
        else
        {
            isMoving = false;
        }
    }

    public Vector3 RamdomPointInSphere(float radius)
    {
        Vector3 getPoint = Random.onUnitSphere;
        getPoint.y = 0f;

        float r = Random.Range(0f, radius);

        return (getPoint * r) + duckarea.transform.position;
    }
}
