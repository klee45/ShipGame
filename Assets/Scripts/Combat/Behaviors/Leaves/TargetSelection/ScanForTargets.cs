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
        DetectionShip detection = state.GetShipDetections();
        if (detection.Scan())
        {
            return NodeState.Success;
        }
        else
        {
            detection.IncreaseRange();
            return NodeState.Failure;
        }
    }
}
