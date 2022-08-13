using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeightedLine : MonoBehaviour
{
    private int weight;
    private Text text;
    private RectTransform pos;
    private RandPosText manager;

    // Start is called before the first frame update
    void Awake()
    {
        text = transform.GetChild(0).GetComponent<Text>();
        pos = transform.GetChild(0).GetComponent<RectTransform>();
        manager = transform.parent.GetComponent<RandPosText>();
        GetComponent<Button>().onClick.AddListener(() => manager.OnClickText(weight));
    }

    public void SetValue(string _line, int _weight)
    {
        text.text = _line;
        weight = _weight;
    }
}
