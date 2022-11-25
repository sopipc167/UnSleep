using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleManager : MonoBehaviour
{
    [Header("참조")]
    public RelationshipManager manager;
    public Text scaleText;
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

    // Start is called before the first frame update
    void Awake()
    {
        scaleRotation = GetComponent<ScaleRotation>();
        ani = GetComponent<Animator>();
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

        if (Weight == 5)
        {
            scaleText.text = unbalance;
            scaleRotation.RotateScale(5);
        }
        else if (Weight == -5)
        {
            scaleText.text = unbalance;
            scaleRotation.RotateScale(-5);
        }
        else
        {
            scaleText.text = Weight.ToString();
            scaleRotation.RotateScale(Weight);
        }

        if (Weight == 0)
        {
            StartCoroutine(ClearCoroutine());
        }
    }

    private IEnumerator ClearCoroutine()
    {
        scaleText.text = balance;
        ani.SetBool("end", true);
        yield return new WaitForSeconds(0.5f);
        scaleRotation.RotateScale(Weight, 1.5f);
        yield return new WaitForSeconds(1.5f);
        manager.CharacterClear(manager.currentType);
        manager.StartScenePause();
        ani.SetBool("end", false);
    }

    public void OnClickBackToA()
    {
        manager.BackToSceneA();
    }
}
