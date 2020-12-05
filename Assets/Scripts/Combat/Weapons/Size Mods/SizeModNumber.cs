using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SizeModNumber : SizeMod
{
    // Not sure why, but create on hit effects
    // only work correctly if this is serialized!
    // Otherwise there are some strange errors
    [Header("Current value")]
    [SerializeField]
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
