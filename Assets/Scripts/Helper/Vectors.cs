using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vectors
{
    public static float AngleBetween(this Vector2 a, Vector2 b)
    {
        return Mathf.Atan2(b.y - a.y, b.x - a.x);
    }

    public static float AngleBetween(this Vector3 a, Vector3 b)
    {
        return AngleBetween(new Vector2(a.x, a.y), new Vector2(b.x, b.y));
    }

    public static Vector3 ToVector3(this Vector2 v)
    {
        return new Vector3(v.x, v.y);
    }
}
