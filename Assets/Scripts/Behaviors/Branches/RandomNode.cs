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

    public override void Reset()
    {
        SelectNewNode();
        base.Reset();
    }

    private void SelectNewNode()
    {
        int pos = Random.Range(0, children.Length - 1);
    }

    protected override string GetName()
    {
        return "Random";
    }
}
