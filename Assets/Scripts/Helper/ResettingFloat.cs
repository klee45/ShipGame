using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResettingFloat
{
    private const bool DEBUG = true;
    private const float MIN_DIFF = 0.05f;

    public readonly float baseValue;
    private float currentValue;
    private int modVal;

    public ResettingFloat(float baseValue)
    {
        this.baseValue = baseValue;
        this.currentValue = baseValue;
        modVal = 0;
    }

    public float GetValue()
    {
        return currentValue;
    }

    public int GetInt()
    {
        return Mathf.RoundToInt(currentValue);
    }

    public void Add(float val)
    {
        currentValue += val;
        modVal += 1;
    }

    public void AddUndo(float val)
    {
        currentValue += val;
        modVal -= 1;
        CheckReset();
    }

    public void Mult(float val)
    {
        currentValue *= val;
        modVal += 1;
    }

    public void MultUndo(float val)
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
                    Debug.Log(string.Format(
                        "Resetting float had a different current ({0}) than base value ({1})",
                        currentValue,
                        baseValue));
                }
            }
            currentValue = baseValue;
        }
    }
}
