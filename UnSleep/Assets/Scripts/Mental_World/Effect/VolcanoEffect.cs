using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoEffect : MonoBehaviour, IEffect
{
    [Range(0.0f, 1.0f)] public float GrayScale = 1.0f;
    [Range(0.0f, 1.0f)] public float GrainStrange = 0.2f;
    [Range(0.0f, 0.01f)] public float JitterStrange = 0.0f;

    public OldCinemaEffect effect;
    public ParticleSystem environmentParticle;
    public ParticleSystem volcanoParticle;

    public void OnEffect()
    {
        effect.GrayScale = GrayScale;
        effect.GrainStrange = GrainStrange;
        effect.JitterStrange = JitterStrange;
        effect.enabled = true;
        environmentParticle.gameObject.SetActive(true);
        volcanoParticle.gameObject.SetActive(true);
        environmentParticle.Play();
        volcanoParticle.Play();
    }
}
