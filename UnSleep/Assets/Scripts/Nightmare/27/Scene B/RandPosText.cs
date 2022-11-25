using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandPosText : MonoBehaviour
{
    [Header("참조")]
    public ScaleManager manager;
    public SceneAText sceneA;

    private WeightedLine[] weightedLines = new WeightedLine[4];
    private TextSizeButton[] buttons = new TextSizeButton[4];

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < 4; ++i)
        {
            var child = transform.GetChild(i);
            weightedLines[i] = child.GetComponent<WeightedLine>();
            buttons[i] = child.GetComponent<TextSizeButton>();
        }
    }

    private struct WL
    {
        public string line;
        public int weight;
    }

    WL[] ws = new WL[4];

    // Knuth Shuffle Algorithm
    private void Shuffle(WL[] deck)
    {
        int size = deck.Length;
        WL temp;
        for (int i = size - 1; i > 0; --i)
        {
            int j = Random.Range(0, i + 1);
            temp = deck[i];
            deck[i] = deck[j];
            deck[j] = temp;
        }
    }

    public void PrintText(CharacterType type, bool isMe)
    { 
        if (isMe)
        {
            ws[0].weight = 9;
            ws[1].weight = 7;
            ws[2].weight = 4;
            ws[3].weight = 2;
        }
        else
        {
            ws[0].weight = -9;
            ws[1].weight = -7;
            ws[2].weight = -4;
            ws[3].weight = -2;
        }

        switch (type)
        {
            case CharacterType.Colleague:
                if (isMe)
                {
                    ws[0].line = "일부러 나 창피하라고 그렇게 말한 거 아니야?";
                    ws[1].line = "상사가 너랑 나를 비교할 때\n네가 짓는 미묘한 미소를 봤어.";
                    ws[2].line = "네 옆에 있으니 내 자신이\n보잘것없고 초라해보여.";
                    ws[3].line = "사회 생활이란 게 원래 다 이런 건가?";
                }
                else
                {
                    ws[0].line = "너 나 싫어하지?";
                    ws[1].line = "나는 사회생활도 잘하고 업무 능력도 뛰어나.";
                    ws[2].line = "우리는 작업 스타일이 다른 것 뿐이야.";
                    ws[3].line = "스스로를 갉아 먹지 마.";
                }
                break;
            case CharacterType.GF:
                if (isMe)
                {
                    ws[0].line = "요즘 우리 관계에 대한 의문이 들어.";
                    ws[1].line = "우린 서로 연락도 뜸해지고\n같이 있는 시간도 줄었어.";
                    ws[2].line = "직장생활을 하다 보니\n시간과 체력이 너무 부족해.";
                    ws[3].line = "예전만큼 잘해주지 못해서 미안해.";
                }
                else
                {
                    ws[0].line = "오빠는 항상 이런 식이야.";
                    ws[1].line = "술 마시러 갔다더니 연락은 또 왜 안되는 거야?";
                    ws[2].line = "직장생활 시작하니까 스트레스 받아서 그런 거지?";
                    ws[3].line = "몸 생각하면서 적당히 마셔.";
                }
                break;
            case CharacterType.Friends:
                if (isMe)
                {
                    ws[0].line = "나도 현실에서 잘하고 싶은데\n재준이는 어떻게 저렇게 잘나갈까.";
                    ws[1].line = "포기했던 내 꿈을 계속 이어가는\n장현이를 보니 후회가 돼.";
                    ws[2].line = "실패할 것 같아 꿈을 포기했는데\n현실을 좇은 내 인생은 성공했나?";
                    ws[3].line = "지금부터라도 조금씩\n취미로라도 그림을 그려봐야지.";
                }
                else
                {
                    ws[0].line = "요즘 일이 술술 잘 풀려서 너무 행복해.";
                    ws[1].line = "지금까지 고생했던 게 드디어\n빛을 보고 있는 것 같아.";
                    ws[2].line = "그렇지만 나이를 먹는 만큼\n개인적인 걱정도 늘어가고 있어.";
                    ws[3].line = "힘들 때는 우리에게 의지해도 돼.";
                }
                break;
            default:
                break;
        }

        Shuffle(ws);

        for (int i = 0; i < 4; ++i)
        {
            weightedLines[i].SetValue(ws[i].line, ws[i].weight);
            buttons[i].RefreshSize();
        }
    }


    public void OnClickText(int weight)
    {
        sceneA.ResetData();
        manager.OnClickBackToA();
        manager.AddWeight(weight);
    }

    public void OnClickBackToA()
    {
        sceneA.ResetData();
        manager.OnClickBackToA();
    }
}
