using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BehaviorConditional : BehaviorLink
{
    [SerializeField]
    protected NodeState exitState = NodeState.Running;

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        if (Conditional(state))
        {
            return child.UpdateState(state);
        }
        else
        {
            return exitState;
        }
    }

    protected abstract bool Conditional(BehaviorState state);
}
