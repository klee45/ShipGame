using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIsAlive : BehaviorConditional
{
    protected override bool Conditional(BehaviorState state)
    {
        return state.target.ship.GetCombatStats().IsAlive();
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        if (Conditional(state))
        {
            child.UpdateState(state);
            return NodeState.RUNNING;
        }
        else
        {
            return failState;
        }
    }

    protected override string GetName()
    {
        return "Target is still alive";
    }
}
