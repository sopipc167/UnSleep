using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleManager : MonoBehaviour
{
    [Header("참조")]
    public RelationshipManager manager;
    public Text scaleText;

    private ScaleRotation scaleRotation;
    private int weight = -10;

    private readonly string unbalance = "불균형";

    // Start is called before the first frame update
    void Awake()
    {
        scaleRotation = GetComponent<ScaleRotation>();
    }

    public void ResetData()
    {
        weight = -7;
        scaleText.text = unbalance;
        scaleRotation.RotateScale(-5);
    }

    public void AddWeight(int value)
    {
        weight += value;

        if (weight > 5)
        {
            scaleText.text = unbalance;
            scaleRotation.RotateScale(5);
        }
        else if (weight < -5)
        {
            scaleText.text = unbalance;
            scaleRotation.RotateScale(-5);
        }
        else
        {
            scaleText.text = weight.ToString();
            scaleRotation.RotateScale(weight);
        }

        if (weight == 0)
        {
            //clear
            manager.CharacterClear(manager.currentType);
            manager.StartScenePause();
        }
    }

    public void OnClickBackToA()
    {
        manager.BackToSceneA();
    }
}
