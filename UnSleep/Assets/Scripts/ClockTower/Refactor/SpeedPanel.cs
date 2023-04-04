﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPanel : MonoBehaviour
{
    public BCogWheel cogWheel;
    public TextMesh blackSpeedText;
    public TextMesh whiteSpeedText;

    private void Start()
    {
        cogWheel.setSpeedPanel(this);
        whiteSpeedText.text = ((int)cogWheel.bInfo.speed).ToString();
    }

    private void OnEnable()
    {
        cogWheel.setSpeedPanel(this);
        whiteSpeedText.text = ((int)cogWheel.bInfo.speed).ToString();
    }

    public void updateBlackSpeedText(float speed)
    {
        if (speed > 0f)
        {
            blackSpeedText.text = ((int)speed).ToString();
        } else
        {
            blackSpeedText.text = "";
        }
    }
}
