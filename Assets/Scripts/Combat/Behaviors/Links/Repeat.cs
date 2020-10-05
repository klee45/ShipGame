using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeat : BehaviorLink
{
    [SerializeField]
    private NodeState repeatState = NodeState.Running;

    protected override string GetName()
    {
        return "Repeating behavior";
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        switch(child.UpdateState(state))
        {
            case NodeState.Success:
            case NodeState.Failure:
                child.ResetNode();
                break;
        }
        return repeatState;
    }
}
