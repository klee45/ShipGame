using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHealthyTarget : BehaviorLeaf
{
    protected override string GetName()
    {
        return "Target most healthy";
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        throw new System.NotImplementedException();
        /*
        if (state.GetShipDetections().GetHealthiest(out Ship ship))
        {
            state.ship = ship;
            return NodeState.Success;
        }
        return NodeState.Failure;
        */
    }
}
