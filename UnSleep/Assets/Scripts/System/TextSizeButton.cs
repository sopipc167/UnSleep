using UnityEngine;
using UnityEngine.UI;

public class TextSizeButton : MonoBehaviour
{
    // UI 내부 텍스트와 UI의 크기가 일치하게 된다.
    //
    // ::기본 사용법::
    // 1. 사용하고자 하는 UI의 설정 변경
    //   Pivot x를 0으로 변경
    //   Text의 Pivot 또한 x를 0으로 변경
    //   Text의 Allignment를 첫번째는 왼쪽정렬, 두번쨰는 중간정렬로 변경
    // 2. 사용하고자 하는 UI에 해당 스크립트를 붙여넣는다.
    // 3. 다른 클래스에서 해당 클래스를 참조한 후, RefreshSize() 함수를 적절히 사용해준다.

    private Text text;
    private RectTransform rect;

    private void Awake()
    {
        //버튼의 가장 첫번쨰 자식은 무조건 Text여야 함.
        text = transform.GetChild(0).GetComponent<Text>();
        rect = GetComponent<RectTransform>();
    }

    //만약 버튼의 텍스트가 변경되어서 갱신이 필요하다면, 해당 함수를 불러준다. (최초 생성 시도 필요)
    public void RefreshSize()
    {
        text.rectTransform.sizeDelta = new Vector2(text.preferredWidth, text.preferredHeight);
        rect.sizeDelta = new Vector2(text.preferredWidth, text.preferredHeight);
    }
}