using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNode : BehaviorBranch
{
    [SerializeField]
    private int pos;

    private void Start()
    {
        SelectNewNode();
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        return children[pos].UpdateState(state);
    }

    public override void ResetNode()
    {
        SelectNewNode();
        base.ResetNode();
    }

    private void SelectNewNode()
    {
        int pos = Random.Range(0, children.Length - 1);
    }

    protected override string GetName()
    {
        return string.Format("Random\n{0}", pos);
    }
}
