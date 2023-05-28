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
    public AudioClip groundSound;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            OnEffect();
        }
    }

    public void OnEffect()
    {
        effect.GrayScale = GrayScale;
        effect.GrainStrange = GrainStrange;
        if (Dialogue_Proceeder.instance.CurrentEpiID == 4 ||
            Dialogue_Proceeder.instance.CurrentEpiID == 13)
        {
            effect.JitterStrange = JitterStrange * 2;
        }
        else if (Dialogue_Proceeder.instance.CurrentEpiID == 5)
        {
            effect.JitterStrange = JitterStrange;
        }
        else
        {
            effect.JitterStrange = 0f;
        }
        effect.enabled = true;
        towerObj.OnEffect();

        if (Dialogue_Proceeder.instance.CurrentEpiID == 4 ||
            Dialogue_Proceeder.instance.CurrentEpiID == 13)
        {
            SoundManager.Instance.PlaySE(groundSound);
        }
        else
        {
            StartCoroutine(PlaySoundCoroutine());
        }
    }

    private IEnumerator PlaySoundCoroutine()
    {
        while (true)
        {
            SoundManager.Instance.PlaySE(ticSound);
            if (Dialogue_Proceeder.instance.CurrentEpiID == 8 ||
                Dialogue_Proceeder.instance.CurrentEpiID == 14)
            {
                yield return new WaitForSeconds(0.5f);
            }
            else if (Dialogue_Proceeder.instance.CurrentEpiID == 12)
            {
                yield return new WaitForSeconds(2.5f);
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
