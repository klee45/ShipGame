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
        //Debug.Log(state.ship.name + " Scan!");
        DetectionShip detection = state.GetShipDetections();
        if (detection.Scan())
        {
            //Debug.Log(state.ship.name + " Scan successful");
            return NodeState.Success;
        }
        else
        {
            //Debug.Log(state.ship.name + " Scan failure");
            detection.IncreaseRange();
            return NodeState.Failure;
        }
    }
}
