using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interaction : MonoBehaviour
{
    Collider[] Hit_colliders;
    public AudioClip Shake_Sound;
    AudioSource audioSource;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Hit_colliders = Physics.OverlapSphere(transform.position, 5.0f);

        if (Hit_colliders.Length>0)
        {
            for (int i = 0; i < Hit_colliders.Length; i++)
            {
                if (Hit_colliders[i].tag == "Interaction")
                {
                   // Dialogue_Proceeder.instance.Dia_Complete_Condition = Hit_colliders[i].GetComponent<Info>().Complete_Condition;
                    //Dialogue_Proceeder dialogue_Proceeder = GameObject.Find("DiaProceeder").GetComponent<Dialogue_Proceeder>();
                    //dialogue_Proceeder.Dia_Complete_Condition = Hit_colliders[i].GetComponent<Info>().Complete_Condition;

                    if (Hit_colliders[i].GetComponent<Info>().CameraShake)
                    {
                        Camera.main.GetComponent<CameraShakeShake>().CamerShake(4f, 0.5f);
                        audioSource.clip = Shake_Sound;
                        audioSource.Play();
                        Hit_colliders[i].GetComponent<Info>().CameraShake = false;
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
