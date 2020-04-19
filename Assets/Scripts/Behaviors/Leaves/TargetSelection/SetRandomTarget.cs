using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRandomTarget : BehaviorLeaf
{
    protected override string GetName()
    {
        return "Set random target";
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        Detection detections = state.GetDetections();
        bool result = detections.GetMemoryDict().GetRandom(ref state.ship);
        if (result)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
