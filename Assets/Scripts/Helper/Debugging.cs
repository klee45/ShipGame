using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Debugging
{
    public static readonly bool SHOW_DETECTION_ZONE = true;

    public static string Print<T>(this List<T> lst, string delimiter)
    {
        List<string> strings = new List<string>();
        foreach (T val in lst)
        {
            strings.Add(val.ToString());
        }
        return System.String.Join(delimiter, strings);
    }
}
