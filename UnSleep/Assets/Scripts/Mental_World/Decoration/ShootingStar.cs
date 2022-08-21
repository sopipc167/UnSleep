using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingStar : MonoBehaviour
{
    public Vector3 destination;
    public float speed;

    private Vector3 curPos;
    private ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        curPos = transform.localPosition;
        StartCoroutine(ShootingCoroutine());
    }

    private IEnumerator ShootingCoroutine()
    {
        while (true)
        {
            transform.localPosition = curPos;
            particle.Play();
            Vector3 delta;
            Vector3 end = Vector3.zero;
            do
            {
                delta = speed * Time.deltaTime * destination;
                end += delta;
                transform.localPosition += delta;
                yield return null;
            } while (end.sqrMagnitude < destination.sqrMagnitude);
            particle.Stop();
            yield return new WaitForSeconds(Random.Range(5, 10));
        }
    }
}
