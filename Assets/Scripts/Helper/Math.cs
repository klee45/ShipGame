using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Math
{
    public static float ToDegree(this float f)
    {
        return f * 180f / Mathf.PI;
    }

    public static float ToRadian(this float f)
    {
        return f * Mathf.PI / 180f;
    }
}
