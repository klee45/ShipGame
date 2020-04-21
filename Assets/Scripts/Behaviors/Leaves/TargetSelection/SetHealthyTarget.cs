﻿using System.Collections;
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
        if (state.GetDetections().GetMemoryDict().GetHealthiest(out Ship ship))
        {
            state.ship = ship;
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}