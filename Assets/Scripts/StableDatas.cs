using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StableDatas
{
    public static Dictionary<int, float> delayLevels = new()
    {
        { 1, 1f },
        { 2, 0.7f },
        { 3, 0.5f }
    };

    public static Dictionary<int, float> personSpeedLevels = new()
    {
        { 1, 8f },
        { 2, 13f },
        { 3, 17f }
    };

    public static Dictionary<int, int> lineCountLevels = new()
    {
        { 1, 1 },
        { 2, 2 },
        { 3, 3 }
    };

    public static float radiusOfPersonSpawn = 2f;
}
