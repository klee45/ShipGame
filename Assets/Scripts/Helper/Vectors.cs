using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vectors
{
    public static Vector2 ToVector2(this Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }

    public static float ToAngle(this Vector2 a)
    {
        return Mathf.Atan2(a.y, a.x);
    }

    public static float AngleBetween(this Vector2 a, Vector2 b)
    {
        return Mathf.Atan2(b.y - a.y, b.x - a.x);
    }

    public static float AngleBetween(this Vector3 a, Vector3 b)
    {
        return AngleBetween(new Vector2(a.x, a.y), new Vector2(b.x, b.y));
    }

    public static Vector2 AngleToVector(this float angle)
    {
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    public static Vector3 ToVector3(this Vector2 v)
    {
        return new Vector3(v.x, v.y);
    }

    public static Vector3 NewX(this Vector3 v, float x)
    {
        return new Vector3(x, v.y, v.z);
    }

    public static Vector3 NewY(this Vector3 v, float y)
    {
        return new Vector3(v.x, y, v.z);
    }

    public static Vector3 NewZ(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }
}
