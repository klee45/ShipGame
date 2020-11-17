using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorHelper
{
    public static Color SetAlpha(this Color c, float a)
    {
        return new Color(c.r, c.g, c.b, a);
    }
}
