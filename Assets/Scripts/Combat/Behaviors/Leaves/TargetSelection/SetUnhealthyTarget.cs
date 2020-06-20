using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUnhealthyTarget : BehaviorLeaf
{
    protected override string GetName()
    {
        return "Target least healthy";
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        if (state.GetShipDetections().GetLeastHealthy(out Ship ship))
        {
            state.ship = ship;
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
