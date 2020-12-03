using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    public static bool IsPrefab(this GameObject obj)
    {
        return obj.scene.name == null;
    }
}
