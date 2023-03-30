using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    private static bool isPause;
    public static bool IsPause
    {
        get => isPause;
        set
        {
            Time.timeScale = value ? 0f : 1f;
            isPause = value;
        }
    }

    public static bool ClickableMemo
    {
        get;
        set;
    }
}
