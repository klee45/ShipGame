using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatGroupZeroOrder : StatGroup
{
    public override float GetValue()
    {
        return 0;
    }

    public override void MultMod(float inc, float dec)
    {
    }

    public override void MultModUndo(float inc, float dec)
    {
    }

    public override void Tick(float scale, float deltaTime)
    {
    }
}
