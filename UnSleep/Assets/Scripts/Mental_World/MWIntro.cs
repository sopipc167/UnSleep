using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MWIntro : MonoBehaviour
{

    public GameObject DiaUI;

    private void OnEnable() //인트로 재생
    {
        DiaUI.SetActive(false);
        GetComponent<PlayableDirector>().Play();
    }

    public void DisableMWIntro()
    {
        DiaUI.SetActive(true);
        this.gameObject.SetActive(false);

    }
}
