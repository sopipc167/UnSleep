using System.Collections;
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
        updateBlackSpeedText((int)cogWheel.info.speed);
        whiteSpeedText.text =((int)cogWheel.bInfo.speed).ToString();

        if (cogWheel.bInfo.type == BCogWheelType.DOWNSPEED)
            whiteSpeedText.text = whiteSpeedText.text.ToString() + "↓";
        else if (cogWheel.bInfo.type == BCogWheelType.UPSPEED)
            whiteSpeedText.text = whiteSpeedText.text.ToString() + "↑";
    }

    private void OnEnable()
    {
        cogWheel.setSpeedPanel(this);
        updateBlackSpeedText((int)cogWheel.info.speed);
        whiteSpeedText.text = ((int)cogWheel.bInfo.speed).ToString();

        if (cogWheel.bInfo.type == BCogWheelType.DOWNSPEED)
            whiteSpeedText.text = whiteSpeedText.text.ToString() + "↓";
        else if (cogWheel.bInfo.type == BCogWheelType.UPSPEED)
            whiteSpeedText.text = whiteSpeedText.text.ToString() + "↑";
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

    public void updateBlackSpeedText(int speed)
    {
        if (speed > 0)
        {
            blackSpeedText.text = speed.ToString();
        }
        else
        {
            blackSpeedText.text = "";
        }
    }
}
