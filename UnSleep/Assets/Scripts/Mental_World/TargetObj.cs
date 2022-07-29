using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TowardPlayer))]
public class TargetObj : MonoBehaviour
{
    private Light objLight;
    private ParticleSystem particle;
    private Coroutine endCoroutine;
    private Coroutine setCoroutine;

    public void InitLight(float range, Color color)
    {
        objLight = transform.GetChild(0).GetComponent<Light>();
        particle = transform.GetChild(1).GetComponent<ParticleSystem>();
        objLight.intensity = 0f;
        objLight.range = range;
        objLight.color = color;
    }

    public void SetTarget(float deltaLight, float maxVal, float minVal)
    {
        if (endCoroutine != null) StopCoroutine(endCoroutine);
        objLight.gameObject.SetActive(true);
        particle.gameObject.SetActive(true);
        particle.Play();
        setCoroutine = StartCoroutine(SetTargetCoroutine(deltaLight, maxVal, minVal));
    }

    public void StopTarget(float deltaLight)
    {
        if (setCoroutine != null) StopCoroutine(setCoroutine);
        particle.Stop();
        endCoroutine = StartCoroutine(StopTargetCoroutine(deltaLight));
    }

    private IEnumerator SetTargetCoroutine(float deltaLight, float maxVal, float minVal)
    {
        while (true)
        {
            while (objLight.intensity < maxVal)
            {
                objLight.intensity += deltaLight * Time.deltaTime;
                yield return null;
            }
            while (objLight.intensity > minVal)
            {
                objLight.intensity -= deltaLight * Time.deltaTime;
                yield return null;
            }
        }
    }

    private IEnumerator StopTargetCoroutine(float deltaLight)
    {
        while (objLight.intensity >= 0f)
        {
            objLight.intensity -= deltaLight * Time.deltaTime;
            yield return null;
        }
        objLight.gameObject.SetActive(false);
        particle.gameObject.SetActive(false);
    }
}
