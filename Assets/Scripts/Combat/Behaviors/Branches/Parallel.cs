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
            case BranchType.And:
                return BranchAnd(state);
            case BranchType.Or:
                return BranchOr(state);
        }
        return NodeState.Error;
    }

    private NodeState BranchAnd(BehaviorState state)
    {
        bool someRunning = false;
        foreach (BehaviorNode child in children)
        {
            NodeState result = child.UpdateState(state);
            switch (result)
            {
                case NodeState.Success:
                    break;
                case NodeState.Running:
                    someRunning = true;
                    break;
                case NodeState.Failure:
                     return NodeState.Failure;
            }
        }
        if (someRunning)
        {
            return NodeState.Running;
        }
        else
        {
            return NodeState.Success;
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
                case NodeState.Success:
                    return NodeState.Success;
                case NodeState.Failure:
                    break;
                case NodeState.Running:
                    someRunning = true;
                    break;
            }
        }
        if (someRunning)
        {
            return NodeState.Running;
        }
        else
        {
            return NodeState.Failure;
        }
    }

    protected override string GetName()
    {
        return string.Format("Parallel\n({0})", type.ToString().ToLower());
    }
}
