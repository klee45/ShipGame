using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelUntilFirstChild : BehaviorBranch
{
    protected override string GetName()
    {
        return "Parallel until condition";
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        NodeState result = children[0].UpdateState(state);
        switch(result)
        {
            case NodeState.SUCCESS:
                return NodeState.SUCCESS;
            default:
                for (int i = 1; i < children.Length; i++)
                {
                    children[i].UpdateState(state);
                }
                return NodeState.RUNNING;
        }
    }
}
