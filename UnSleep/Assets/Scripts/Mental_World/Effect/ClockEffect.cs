using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockEffect : MonoBehaviour, IEffect
{
    [Range(0.0f, 1.0f)] public float GrayScale = 1.0f;
    [Range(0.0f, 1.0f)] public float GrainStrange = 0.2f;
    [Range(0.0f, 0.01f)] public float JitterStrange = 0.0f;

    public OldCinemaEffect effect;
    public ClockTowerObject towerObj;
    public AudioClip ticSound;

    public void OnEffect()
    {
        effect.GrayScale = GrayScale;
        effect.GrainStrange = GrainStrange;
        effect.JitterStrange = JitterStrange;
        effect.enabled = true;
        towerObj.OnEffect();
        StartCoroutine(PlaySoundCoroutine());
    }

    private IEnumerator PlaySoundCoroutine()
    {
        while (true)
        {
            SoundManager.Instance.PlaySE(ticSound);
            yield return new WaitForSeconds(Random.Range(0.8f, 1f));
        }
    }
}
