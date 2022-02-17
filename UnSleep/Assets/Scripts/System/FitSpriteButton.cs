using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FitSpriteButton : MonoBehaviour
{
    // ::용도::
    // 버튼이미지의 투명한 부분은 클릭해도 버튼이 작동하지 않는다.
    //
    // ::기본 사용법::
    // 1. 사용하고자 하는 이미지의 설정 변경 (컴포넌트가 아니라 이미지 에셋 설정임)
    //   Sprite Mode - Mesh Type 을 Full Rect
    //   Advanced - Read/Write Enabled 를 true로
    // 2. 사용하고자 하는 버튼에 해당 스크립트를 붙여넣으면 됨
    //
    // ::주의점::
    // 헤당 이미지 설정은 이미지 최적화에 미미한 영향을 미칠 수 있음 (남용X)

    void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }
}
