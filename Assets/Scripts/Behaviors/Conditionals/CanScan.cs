using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanScan : BehaviorConditional
{
    protected override bool Conditional(BehaviorState state)
    {
        return state.GetDetections().CanScan();
    }

    protected override string GetName()
    {
        return "Can scan";
    }
}
