using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatGroupZeroOrder : StatGroup
{
    public override float GetValue()
    {
        return 0;
    }

    public override float GetValue(float duration)
    {
        return 0;
    }

    public override void Tick(float scale)
    {
    }
}
