using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Script : MonoBehaviour //버튼이 하는일에 대한 스크립트
{
    public void Quit() //게임 나가기
    {
        Application.Quit();
    }
    public void Restart() //게임 재시작
    {
        SceneChanger.ChangeScene(SceneType.Volcano);
    }
}