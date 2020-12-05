using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SizeModNumber : SizeMod
{
    protected float value;

    public float ToFloat()
    {
        return value;
    }

    public int ToInt()
    {
        return (int)ToFloat();
    }
}
