using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleClear : MonoBehaviour
{
    private float _startDelay;
    private float _endDelay;
    [SerializeField] private Animator ani;

    public void ClearPuzzle(float startDelay = 0f, float endDelay = 2f)
    {
        _startDelay = startDelay;
        _endDelay = endDelay < 2f ? 2f : endDelay;
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        StartCoroutine(ClearCoroutine()); 
    }

    private IEnumerator ClearCoroutine()
    {
        if (_startDelay != 0f)
        {
            yield return new WaitForSeconds(_startDelay);
        }

        ani.SetBool("isStart", true);
        yield return new WaitForSeconds(_endDelay);

        Dialogue_Proceeder.instance.UpdateCurrentDiaIDPlus1(); //씬 이동 후 다음 대사를 말하기 위해 하나 슬쩍 넣었습니다
        SceneManager.LoadScene("Mental_World_Map");
    }
}