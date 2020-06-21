using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ATimeScale : MonoBehaviour
{
    protected float scale = 1;

    public float GetScale()
    {
        return scale;
    }

    public abstract void ResetScale();
    public abstract void ChangeMult(int add);
    public abstract void ChangeDiv(int add);

    public enum TimeScaleType
    {
        STANDARD,
        STATIC
    }
}
