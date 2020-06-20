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
            case BranchType.AND:
                return AndBranch(childResult);
            case BranchType.OR:
                return OrBranch(childResult);
        }
        return NodeState.ERROR;
    }

    private NodeState AndBranch(NodeState childResult)
    {
        switch (childResult)
        {
            case NodeState.SUCCESS:
                if (++pos >= len)
                {
                    return Succeed();
                }
                return NodeState.RUNNING;
            case NodeState.FAILURE:
                return Failure();
            case NodeState.RUNNING:
                return NodeState.RUNNING;
        }
        return NodeState.ERROR;
    }

    private NodeState OrBranch(NodeState childResult)
    {
        switch (childResult)
        {
            case NodeState.SUCCESS:
                return Succeed();
            case NodeState.FAILURE:
                if (++pos >= len)
                {
                    return Failure();
                }
                 return NodeState.RUNNING;
            case NodeState.RUNNING:
                return NodeState.RUNNING;
        }
        return NodeState.ERROR;
    }

    private NodeState Failure()
    {
        ResetSelf();
        return NodeState.FAILURE;
    }

    private NodeState Succeed()
    {
        ResetSelf();
        return NodeState.SUCCESS;
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
