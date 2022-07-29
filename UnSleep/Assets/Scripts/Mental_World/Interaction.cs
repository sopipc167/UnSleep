using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    Collider[] Hit_colliders;
    public AudioClip Shake_Sound;

    private CameraShakeShake camShake;


    private void Start()
    {
        camShake = Camera.main.GetComponent<CameraShakeShake>();
    }

    void FixedUpdate()
    {
        Hit_colliders = Physics.OverlapSphere(transform.position, 5.0f);

        if (Hit_colliders.Length>0)
        {
            for (int i = 0; i < Hit_colliders.Length; i++)
            {
                if (Hit_colliders[i].CompareTag("Interaction"))
                {
                    // Dialogue_Proceeder.instance.Dia_Complete_Condition = Hit_colliders[i].GetComponent<Info>().Complete_Condition;
                    //Dialogue_Proceeder dialogue_Proceeder = GameObject.Find("DiaProceeder").GetComponent<Dialogue_Proceeder>();
                    //dialogue_Proceeder.Dia_Complete_Condition = Hit_colliders[i].GetComponent<Info>().Complete_Condition;

                    Info info = Hit_colliders[i].GetComponent<Info>();
                    if (info.CameraShake)
                    {
                        camShake.CamerShake(4f, 0.5f);
                        SoundManager.Instance.PlaySE(Shake_Sound);
                        info.CameraShake = false;
                    }

                    //if (Dialogue_Proceeder.instance.Dia_Complete_Condition.Equals("1806"))
                    //{
                     //   SceneManager.LoadScene("ClockTower");
                    //}
                }
            }
        }
    }
}
