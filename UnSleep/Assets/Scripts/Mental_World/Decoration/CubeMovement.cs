using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    public float scale;
    public float minCyclePerSecond;
    public float maxCyclePerSecond;

    private float speed;
    private float yValue;

    private float sinValue = 0f;

    private void Awake()
    {
        speed = Random.Range(minCyclePerSecond, maxCyclePerSecond) * Mathf.PI * 2;
        yValue = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        sinValue += Time.deltaTime * speed;
        transform.localPosition = new Vector3(transform.localPosition.x, yValue + scale * Mathf.Sin(sinValue), transform.localPosition.z);
    }
}
