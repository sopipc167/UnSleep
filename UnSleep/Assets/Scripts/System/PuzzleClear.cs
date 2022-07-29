using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleClear : MonoBehaviour
{
    private float startDelay;
    private float endDelay;
    private SceneType sceneType;
    [SerializeField] private Animator ani;

    public void ClearPuzzle(SceneType type = SceneType.Mental, float startDelay = 0f, float endDelay = 2f)
    {
        sceneType = type;
        this.startDelay = startDelay;
        this.endDelay = endDelay < 2f ? 2f : endDelay;
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        StartCoroutine(ClearCoroutine()); 
    }

    private IEnumerator ClearCoroutine()
    {
        if (startDelay != 0f)
        {
            yield return new WaitForSeconds(startDelay);
        }

        ani.SetBool("isStart", true);
        yield return new WaitForSeconds(endDelay);

        Dialogue_Proceeder.instance.ClearPuzzle();
        Dialogue_Proceeder.instance.UpdateCurrentDiaIDPlus1(); //씬 이동 후 다음 대사를 말하기 위해 하나 슬쩍 넣었습니다
        SceneChanger.ChangeScene(sceneType);
    }
}