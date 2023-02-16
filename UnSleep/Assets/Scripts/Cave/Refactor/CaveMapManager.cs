using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveMapManager : MonoBehaviour
{
    public TextAsset caveCsv;
    public Cavern rootCavern;
    public CaveMapRenderer caveMapRenderer;

    private CaveMapParser caveMapParser = new CaveMapParser();

    private void Start()
    {
        rootCavern = caveMapParser.getRootCavern(caveCsv);
        caveMapRenderer.renderCavern(rootCavern);
    }
    
}
