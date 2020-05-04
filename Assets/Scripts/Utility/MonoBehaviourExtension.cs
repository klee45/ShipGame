using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviourExtension
{
    public static void SetParent(this MonoBehaviour child, GameObject parent)
    {
        child.transform.SetParent(parent.transform);
    }
}
