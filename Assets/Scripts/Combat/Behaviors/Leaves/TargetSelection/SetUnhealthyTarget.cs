﻿using System.Collections;
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
        throw new System.NotImplementedException();
        /*
        if (state.GetShipDetections().GetLeastHealthy(out Ship ship))
        {
            state.ship = ship;
            return NodeState.Success;
        }
        return NodeState.Failure;
        */
    }
}
