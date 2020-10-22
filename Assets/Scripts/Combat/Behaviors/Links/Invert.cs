using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invert : BehaviorLink
{
    protected override string GetName()
    {
        return "Invert";
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        NodeState result = child.UpdateState(state);
        switch (result)
        {
            case NodeState.Success:
                return NodeState.Failure;
            case NodeState.Failure:
                return NodeState.Success;
            default:
                return result;
        }
    }
}
