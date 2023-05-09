using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public TargetObjManager objManager;

    public GameObject lake;
    public GameObject cliff;
    public GameObject clock;
    public GameObject volcano;
    public GameObject cave;

    public void OnEffect()
    {
        GameObject curEffect = null;
        switch (objManager.CurScene)
        {
            case SceneType.Lake:
                curEffect = lake;
                break;
            case SceneType.Volcano:
                curEffect = volcano;
                break;
            case SceneType.ClockTower:
                curEffect = clock;
                break;
            case SceneType.Cave:
                curEffect = cave;
                break;
            case SceneType.Cliff:
                curEffect = cliff;
                break;
            default:
                break;
        }

        if (curEffect != null)
        {
            curEffect.GetComponent<IEffect>().OnEffect();
        }
    }
}
