using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockEffect : MonoBehaviour, IEffect
{
    public OldCinemaEffect effect;
    public ClockTowerObject towerObj;
    public AudioClip ticSound;

    public void OnEffect()
    {
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
