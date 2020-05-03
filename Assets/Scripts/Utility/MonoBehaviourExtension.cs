using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviourExtension
{
    public static void SetParent(this MonoBehaviour obj, GameObject child)
    {
        child.transform.SetParent(obj.transform);
    }
}
