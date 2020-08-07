using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequential : BehaviorBranch
{
    [SerializeField]
    protected BranchType type;

    private int pos = 0;
    private int len;

    private void Start()
    {
        len = children.Length;
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        NodeState childResult = children[pos].UpdateState(state);
        switch (type)
        {
            case BranchType.And:
                return AndBranch(childResult);
            case BranchType.Or:
                return OrBranch(childResult);
        }
        return NodeState.Error;
    }

    private NodeState AndBranch(NodeState childResult)
    {
        switch (childResult)
        {
            case NodeState.Success:
                if (++pos >= len)
                {
                    return Succeed();
                }
                return NodeState.Running;
            case NodeState.Failure:
                return Failure();
            case NodeState.Running:
                return NodeState.Running;
        }
        return NodeState.Error;
    }

    private NodeState OrBranch(NodeState childResult)
    {
        switch (childResult)
        {
            case NodeState.Success:
                return Succeed();
            case NodeState.Failure:
                if (++pos >= len)
                {
                    return Failure();
                }
                 return NodeState.Running;
            case NodeState.Running:
                return NodeState.Running;
        }
        return NodeState.Error;
    }

    private NodeState Failure()
    {
        ResetSelf();
        return NodeState.Failure;
    }

    private NodeState Succeed()
    {
        ResetSelf();
        return NodeState.Success;
    }

    private void ResetSelf()
    {
        pos = 0;
    }

    public override void ResetNode()
    {
        ResetSelf();
        base.ResetNode();
    }

    protected override string GetName()
    {
        return string.Format("Sequential\n({0})", type.ToString().ToLower());
    }
}
