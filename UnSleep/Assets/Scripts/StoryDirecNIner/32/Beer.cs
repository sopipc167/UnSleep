using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beer : StoryInteract
{
    public BeerImg img;

    public override bool IsCompelete()
    {
        return img.result;
    }
}
