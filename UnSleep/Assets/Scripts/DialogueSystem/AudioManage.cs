using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManage : MonoBehaviour
{

    //완~~~~~~전 임시 연출 작업할 때 음악/효과음 체계도 생각해봐야 할 듯 

    AudioSource audioSource;
    public AudioClip[] sample_sound;
    private int already_played = 0;
    private int Dia_index;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Dia_index = Dialogue_Proceeder.instance.CurrentDiaID;

        if (Dia_index != already_played)
            if (Dia_index!=1809 && Dia_index != 1810)
                 audioSource.Stop();

        if (Dia_index == 1801 && already_played < Dia_index)
        {
            audioSource.clip = sample_sound[0]; //째깍째깍
            already_played = Dia_index;
            audioSource.Play();
        }
        else if (Dia_index == 1802 && already_played < Dia_index)
        {
            audioSource.clip = sample_sound[1]; //아이들 소리
            already_played = Dia_index;
            audioSource.Play();

        }
        else if (Dia_index == 1804 && already_played < Dia_index)
        {
            audioSource.clip = sample_sound[2]; //밤 같은 브금
            already_played = Dia_index;
            audioSource.Play();

        }
        else if (Dia_index == 1805 && already_played < Dia_index)
        {
            audioSource.clip = sample_sound[3]; //째깎재깎 에코ver 
            already_played = Dia_index;
            audioSource.Play();

        }
        else if (Dia_index == 1810 && already_played < Dia_index)
        {
            audioSource.clip = sample_sound[4]; //개운한 음악 
            already_played = Dia_index;
            audioSource.Play();

        }
    }
}
