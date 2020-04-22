using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallel : BehaviorBranch
{
    [SerializeField]
    protected BranchType type;

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        switch (type)
        {
            case BranchType.AND:
                return BranchAnd(state);
            case BranchType.OR:
                return BranchOr(state);
        }
        return NodeState.ERROR;
    }

    private NodeState BranchAnd(BehaviorState state)
    {
        bool someRunning = false;
        foreach (BehaviorNode child in children)
        {
            NodeState result = child.UpdateState(state);
            switch (result)
            {
                case NodeState.SUCCESS:
                    break;
                case NodeState.RUNNING:
                    someRunning = true;
                    break;
                case NodeState.FAILURE:
                     return NodeState.FAILURE;
            }
        }
        if (someRunning)
        {
            return NodeState.RUNNING;
        }
        else
        {
            return NodeState.SUCCESS;
        }
    }

    private NodeState BranchOr(BehaviorState state)
    {
        bool someRunning = false;
        foreach (BehaviorNode child in children)
        {
            NodeState result = child.UpdateState(state);
            switch (result)
            {
                case NodeState.SUCCESS:
                    return NodeState.SUCCESS;
                case NodeState.FAILURE:
                    break;
                case NodeState.RUNNING:
                    someRunning = true;
                    break;
            }
        }
        if (someRunning)
        {
            return NodeState.RUNNING;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }

    protected override string GetName()
    {
        return string.Format("Parallel\n({0})", type.ToString().ToLower());
    }
}
