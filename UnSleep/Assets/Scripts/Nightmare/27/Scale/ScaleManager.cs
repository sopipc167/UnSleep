using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleManager : MonoBehaviour
{
    [Header("참조")]
    public RelationshipManager manager;
    public Text scaleText;
    public GameObject domun;
    public GameObject opposite;
    private Animator ani;

    private ScaleRotation scaleRotation;
    private int weight = -5;
    private int Weight
    {
        get => weight;
        set
        {
            if (value < -5) weight = -5;
            else if (value > 5) weight = 5;
            else weight = value;
        }
    }

    private readonly string unbalance = "불균형";
    private readonly string balance = "균형";

    private Text[] domunText = new Text[3];
    private Text[] oppositeText = new Text[3];
    List<string> domunSelectedTexts;
    List<string> oppositeSelectedTexts;
    private IEnumerator setTextCoroutine;

    void Awake()
    {
        scaleRotation = GetComponent<ScaleRotation>();
        ani = GetComponent<Animator>();

        for (int i = 0; i < 3; ++i)
        {
            domunText[i] = domun.transform.GetChild(i).GetComponent<Text>();
            oppositeText[i] = opposite.transform.GetChild(i).GetComponent<Text>();
        }
    }

    public void SetTextInit(string line)
    {
        var parsedLine = new List<string>(line.Split(new char[2] { '\n', ' ' }));
        oppositeSelectedTexts = parsedLine;
        setTextCoroutine = SetTextCoroutine(oppositeSelectedTexts, oppositeText);
        StartCoroutine(setTextCoroutine);
    }

    public void SetText(WeightedLine line)
    {
        if (setTextCoroutine != null) 
            StopCoroutine(setTextCoroutine);

        if (Weight == 0)
        {
            for (int i = 0; i < 3; ++i)
            {
                domunText[i].text = "";
                oppositeText[i].text = "";
            }
            return;
        }

        var parsedLine = new List<string>(line.GetParseText());
        if (line.isDomun)
        {
            domunSelectedTexts = parsedLine;

        }
        else
        {
            oppositeSelectedTexts = parsedLine;
        }

        if (Weight > 0)
        {
            setTextCoroutine = SetTextCoroutine(domunSelectedTexts, domunText);
            for (int i = 0; i < 3; ++i)
            {
                oppositeText[i].text = "";
            }
        }
        else
        {
            setTextCoroutine = SetTextCoroutine(oppositeSelectedTexts, oppositeText);
            for (int i = 0; i < 3; ++i)
            {
                domunText[i].text = "";
            }
        }
        StartCoroutine(setTextCoroutine);
    }

    private IEnumerator SetTextCoroutine(List<string> parsedLine, Text[] texts)
    {
        int[] randIndex = new int[parsedLine.Count];
        for (int i = 0; i < parsedLine.Count; ++i) randIndex[i] = i;
        List<int> selectedIndex = new List<int>() { new int(), new int(), new int()};

        while (true)
        {
            for (int i = randIndex.Length - 1; i > 0; --i)
            {
                int j = Random.Range(0, i + 1);
                int temp = randIndex[i];
                randIndex[i] = randIndex[j];
                randIndex[j] = temp;
            }

            selectedIndex[0] = randIndex[0];
            selectedIndex[1] = randIndex[1];
            selectedIndex[2] = randIndex[2];
            selectedIndex.Sort();

            for (int i = 0; i < 3; ++i)
            {
                texts[i].text = parsedLine[selectedIndex[i]];
            }

            float randTime = Random.Range(0.7f, 1.2f);
            float time = 0f;
            while (time < randTime)
            {
                time += Time.deltaTime;
                yield return null;
            }
        } 
    }

    public void ResetData()
    {
        Weight = -5;
        scaleText.text = unbalance;
        scaleRotation.RotateScale(-5);
    }

    public void AddWeight(int value)
    {
        Weight += value;

        if (Weight == 5 || Weight == -5)
        {
            scaleText.text = unbalance;
            scaleRotation.RotateScale(Weight);
        }
        else if (Weight == 0)
        {
            StartCoroutine(ClearCoroutine());
        }
        else
        {
            scaleText.text = Weight.ToString();
            scaleRotation.RotateScale(Weight);
        }
    }

    private IEnumerator ClearCoroutine()
    {
        manager.PlayClearSound();
        ani.SetBool("end", true);
        scaleText.text = balance;
        scaleRotation.RotateScale(Weight, 0.5f);
        yield return new WaitForSeconds(2f);
        manager.CharacterClear(RelationshipManager.CurrentType);
        ani.SetBool("end", false);
        manager.ClearPhase();
        yield return new WaitForSeconds(0.5f);
        manager.AfterAnimationProcess();
    }

    public void OnClickBackToA()
    {
        manager.BackToSceneA();
    }
}
