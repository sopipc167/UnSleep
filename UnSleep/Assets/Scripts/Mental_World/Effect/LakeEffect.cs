using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LakeEffect : MonoBehaviour, IEffect
{
    public ParticleSystem tornado;
    public Wave waveParticle;

    public void OnEffect()
    {
        var objs = GameObject.FindGameObjectsWithTag("Effect");
        List<SpriteRenderer> list = new List<SpriteRenderer>();
        foreach (var item in objs)
        {
            list.Add(item.GetComponent<SpriteRenderer>());
        }
        StartCoroutine(OnEffectCoroutine(list));
        tornado.gameObject.SetActive(true);
        tornado.Play();
        waveParticle.IsEffectOn = true;
    }

    private IEnumerator OnEffectCoroutine(List<SpriteRenderer> list)
    {
        float time = 2;
        while (list[0].color.a > 0.001f)
        {
            for (int i =0; i < list.Count; ++i)
            {
                list[i].color -= new Color(0, 0, 0, 1 / time * Time.deltaTime);
            }
            yield return null;
        }
    }
}
