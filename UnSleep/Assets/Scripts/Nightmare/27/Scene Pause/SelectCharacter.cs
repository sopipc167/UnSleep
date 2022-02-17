using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum CharacterType { Colleague, GF, Friends }

[RequireComponent(typeof(EventTrigger), typeof(FitSpriteButton))]
public class SelectCharacter : MonoBehaviour
{
    public CharacterType type;
    private SelectManager manager;
    private Button button;

    private void Awake()
    {
        manager = transform.parent.GetComponent<SelectManager>();
        button = GetComponent<Button>();
        EventTrigger trigger = GetComponent<EventTrigger>();

        //마우스 오버 시 이름 출력
        EventTrigger.Entry entry1 = new EventTrigger.Entry();
        entry1.eventID = EventTriggerType.PointerEnter;
        entry1.callback.AddListener((data) => { OnPointerEnterDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry1);

        //마우스 떠날 때 리셋
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerExit;
        entry2.callback.AddListener((data) => { OnPointerExitDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry2);

        //버튼 클릭 시 이동
        button.onClick.AddListener(() => OnClickStartScene2());
    }

    public void OnPointerEnterDelegate(PointerEventData data)
    {
        button.interactable = manager.ChangeName(type);
    }

    public void OnPointerExitDelegate(PointerEventData data)
    {
        manager.ResetText();
    }

    public void OnClickStartScene2()
    {
        manager.StartSceneA(GetComponent<Image>().sprite);
    }
}
