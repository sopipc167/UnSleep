using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveMapManager : MonoBehaviour
{
    public TextAsset caveCsv;
    private CaveMapParser caveMapParser = new CaveMapParser();
    public Cavern carven;

    private void Awake()
    {
        carven = caveMapParser.getCavern(caveCsv);
    }
    
}
