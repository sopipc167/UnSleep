using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoEffect : MonoBehaviour, IEffect
{
    public ParticleSystem environmentParticle;
    public ParticleSystem volcanoParticle;

    public void OnEffect()
    {
        environmentParticle.gameObject.SetActive(true);
        volcanoParticle.gameObject.SetActive(true);
        environmentParticle.Play();
        volcanoParticle.Play();
    }
}
