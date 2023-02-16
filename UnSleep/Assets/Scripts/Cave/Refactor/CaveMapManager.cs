using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveMapManager : MonoBehaviour
{
    public TextAsset caveCsv;
    public Cavern rootCavern;
    public CaveMapRenderer caveMapRenderer;

  

    private void Start()
    {
        rootCavern = new CaveMapParser().getRootCavern(caveCsv);
        caveMapRenderer.renderCavern(rootCavern);
    }
    
}
