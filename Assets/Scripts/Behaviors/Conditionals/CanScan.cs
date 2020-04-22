using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanScan : BehaviorConditional
{
    protected override bool Conditional(BehaviorState state)
    {
        return state.GetShipDetections().CanScan();
    }

    protected override string GetName()
    {
        return "Can scan";
    }
}
