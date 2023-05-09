using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public float minTime;
    public float maxTime;

    private ParticleSystem wave;
    private Vector3 mid;

    public bool IsEffectOn { get; set; }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        mid = transform.position;
        wave = GetComponent<ParticleSystem>();
        while (true)
        {
            transform.position = new Vector3(mid.x, mid.y, mid.z);
            wave.Play();
            yield return new WaitForSeconds(IsEffectOn ? 0.7f : Random.Range(minTime, maxTime));
        }
    }
}
