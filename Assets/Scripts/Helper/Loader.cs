using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader
{
    public static T[][] LoadFromFolder<T>(string path, params string[] subPaths) where T : UnityEngine.Object
    {
        List<T[]> lst = new List<T[]>();

        foreach (string subPath in subPaths)
        {
            lst.Add(Resources.LoadAll<T>(path + "/" + subPath));
        }
        return lst.ToArray();
    }
}
