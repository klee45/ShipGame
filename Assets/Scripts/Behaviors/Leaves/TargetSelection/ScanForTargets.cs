using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanForTargets : BehaviorLeaf
{
    protected override string GetName()
    {
        return "Scan for targets";
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        if (state.GetShipDetections().Scan())
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
