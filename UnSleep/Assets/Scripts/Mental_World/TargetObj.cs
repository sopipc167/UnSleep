using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObj : MonoBehaviour
{
    [Header("참조")]
    public Transform playerPos;

    private Light objLight;
    private Vector3 basisPos;
    private Vector3 dirToTarget;

    public void SetTarget()
    {
        StartCoroutine(SetTargetCoroutine());
    }

    public void StopTarget()
    {
        StopAllCoroutines();
        StartCoroutine(StopTargetCoroutine());
    }

    // Start is called before the first frame update
    void Start()
    {
        objLight = transform.GetChild(0).GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        basisPos = transform.position;
        basisPos.y = playerPos.position.y;

        dirToTarget = playerPos.position - basisPos;
        transform.rotation = Quaternion.LookRotation(dirToTarget, Vector3.up);

        if (Input.GetKeyDown(KeyCode.A))
        {
            SetTarget();
        }
    }

    private IEnumerator SetTargetCoroutine()
    {
        float tmp = 0.004f;
        while (true)
        {
            objLight.intensity += tmp;
            if (objLight.intensity > 2.5f || objLight.intensity < 1f)
            {
                tmp = -tmp;
            }
            yield return null;
        }
    }

    private IEnumerator StopTargetCoroutine()
    {
        float tmp = 0.004f;
        while (objLight.intensity > 1f)
        {
            objLight.intensity -= tmp;
            yield return null;
        }
    }
}
