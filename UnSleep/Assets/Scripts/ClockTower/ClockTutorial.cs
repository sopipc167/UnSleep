using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockTutorial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<PuzzleTutorial>().SetOnTutorial(0, 3); //테스트 코드. 참고상 포함했어요 지우고 쓰세여
        //GetComponent<PuzzleTutorial>().SetOnTutorial(0, 1); //한 페이지 테스트 코드. 지우고 쓰세여
    }


}
