using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeightedLine : MonoBehaviour
{
    public int Weight { get; private set; }
    public bool isDomun { get; private set; }
    private Text text;

    // Start is called before the first frame update
    void Awake()
    {
        text = transform.GetChild(0).GetComponent<Text>();
        RandPosText randPosText = transform.parent.GetComponent<RandPosText>();
        GetComponent<Button>().onClick.AddListener(() => randPosText.OnClickText(this));
    }

    public void SetValue(string newLine, int newWeight, bool isDomun)
    {
        text.text = newLine;
        Weight = newWeight;
        this.isDomun = isDomun;
    }

    public string[] GetParseText()
    {
        return text.text.Split(new char[2] { ' ', '\n' });
    }
}
