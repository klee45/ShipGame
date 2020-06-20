using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScale
{
    private int mult;
    private int div;
    private float scale;

    public TimeScale()
    {
        Reset();
    }

    public void Reset()
    {
        mult = 1;
        div = 1;
        scale = 1;
    }

    public float GetScale()
    {
        return scale;
    }

    public void ChangeMult(int add)
    {
        mult += add;
        Calcluate();
    }

    public void ChangeDiv(int add)
    {
        div += add;
        Calcluate();
    }

    private void Calcluate()
    {
        scale = mult / div;
    }
}
