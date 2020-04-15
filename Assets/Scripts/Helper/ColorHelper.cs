using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorHelper
{
    public static Color SetAlpha(Color c, float a)
    {
        return new Color(c.r, c.g, c.b, a);
    }
}
