using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static void Shuffle<T>(this IList<T> ts, int start, int end)
    {
        for (var i = start; i < end; ++i)
        {
            var r = Random.Range(i, end+1);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    public static void Shuffle<T>(this IList<T> ts)
    {
        Shuffle<T>(ts, 0, ts.Count - 1);
    }
}
