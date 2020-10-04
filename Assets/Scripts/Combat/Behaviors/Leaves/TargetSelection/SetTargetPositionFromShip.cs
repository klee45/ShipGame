using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTargetPositionFromShip : BehaviorLeaf
{
    protected override string GetName()
    {
        return "Set target from ship";
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        state.targetInfo.position = state.targetInfo.ship.transform.position;
        return NodeState.Success;
    }
}
