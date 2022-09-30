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
        Camera.main.transform.rotation = Quaternion.Euler(new Vector3(10f, 0f, 0f)); //카메라 회전 초기화
        Dialogue_Proceeder.instance.CurrentDiaIndex = 0; //잘 안 나오는데 가끔 빠르게 클릭하면 이전 씬에서 대사 넘겨지는 경우 방지
        this.gameObject.SetActive(false);

    }
}
