using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SizeMod : MonoBehaviour
{
    protected float value;

    public abstract void SetupSlot(Size slotSize);
    public abstract void SetupShip(Size shipSize);

    public float ToFloat()
    {
        return value;
    }

    public int ToInt()
    {
        return (int)ToFloat();
    }
}
