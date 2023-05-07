using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CliffEffect : MonoBehaviour, IEffect
{
    public ParticleSystem cliffParticle;
    public void OnEffect()
    {
        cliffParticle.gameObject.SetActive(true);
        cliffParticle.Play();
    }
}
