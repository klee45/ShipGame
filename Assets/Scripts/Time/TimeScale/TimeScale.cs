using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScale : ATimeScale
{
    private int mult;
    private int div;

    private void Awake()
    {
        ResetScale();
    }

    public override void ResetScale()
    {
        mult = 1;
        div = 1;
        scale = 1;
    }

    public override void ChangeMult(int add)
    {
        mult += add;
        Calcluate();
    }

    public override void ChangeDiv(int add)
    {
        div += add;
        Calcluate();
    }

    private void Calcluate()
    {
        scale = mult / div;
    }
}
