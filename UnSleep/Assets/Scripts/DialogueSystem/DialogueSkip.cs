using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueSkip : MonoBehaviour
{
    public TextManager textManager;
    public AudioClip skipClip;


    public void skip()
    {
        // DiaDic을 받아와서 현재 DiaID를 기준으로
        Dictionary<int, DialogueEvent> DiaDic = textManager.getDiaDic();
        int CurDiaID = Dialogue_Proceeder.instance.CurrentDiaID;
        SoundManager.Instance.PlaySE(skipClip);

        // 1 씬이 바뀌거나 2 테이블이 끝나는 곳을 찾기
        while (true)
        {
            if (!DiaDic.ContainsKey(CurDiaID + 1)) // 후반부 스토리 -> 바로 에피소드 완료 후 일기장으로
            {
                Dialogue_Proceeder.instance.End = true; // 끝났음 true. 일기장에서 보고 자동 페이지 넘김과 후일담 출력
                Dialogue_Proceeder.instance.CurrentDiaIndex = 0;
                SaveDataManager.Instance.SaveEpiProgress(Dialogue_Proceeder.instance.CurrentEpiID + 1); //현재 에피소드 완료 저장
                SceneChanger.Instance.ChangeScene(SceneType.Diary);
                break;
            }
            else if (DiaDic[CurDiaID + 1].SceneNum != 1) // 전반부 스토리 -> 마음 속 씬으로
            {
                Dialogue_Proceeder.instance.CurrentDiaID = CurDiaID;
                Dialogue_Proceeder.instance.CurrentDiaIndex = 0;
                SoundManager.Instance.StopSE();
                textManager.StartLoadStoryMental();
                break;
            }
            else
            {
                CurDiaID++;
            }

        }
    }   
}
