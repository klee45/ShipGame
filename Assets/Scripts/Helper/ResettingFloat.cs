using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResettingFloatFixedMult : ResettingFloat
{
    public ResettingFloatFixedMult(float baseValue) : base(baseValue) {}

    public override void Add(float val) {}
    public override void AddUndo(float val) {}
    public override void Mult(float val) {}
    public override void MultUndo(float val) {}
}

public class ResettingFloat
{
    private const bool DEBUG = true;
    private const float MIN_DIFF = 0.05f;

    private float baseValue;
    private float currentValue;
    private int modVal;

    public ResettingFloat(float baseValue)
    {
        Reset(baseValue);
    }

    public void Reset(float baseValue)
    {
        this.baseValue = baseValue;
        this.currentValue = baseValue;
        modVal = 0;
    }

    public float GetBase()
    {
        return baseValue;
    }

    public float GetValue()
    {
        return currentValue;
    }

    public int GetInt()
    {
        return Mathf.RoundToInt(currentValue);
    }

    public virtual void Add(float val)
    {
        currentValue += val;
        modVal += 1;
    }

    public virtual void AddUndo(float val)
    {
        currentValue += val;
        modVal -= 1;
        CheckReset();
    }

    public virtual void Mult(float val)
    {
        currentValue *= val;
        modVal += 1;
    }

    public virtual void MultUndo(float val)
    {
        currentValue *= val;
        modVal -= 1;
        CheckReset();
    }

    private void CheckReset()
    {
        if (modVal == 0)
        {
            if (DEBUG)
            {
                float diff = Mathf.Abs(currentValue - baseValue);
                if (diff > MIN_DIFF)
                {
                    Debug.LogWarning(string.Format(
                        "Resetting float had a different current ({0}) than base value ({1})",
                        currentValue,
                        baseValue));
                }
            }
            currentValue = baseValue;
        }
    }
}
