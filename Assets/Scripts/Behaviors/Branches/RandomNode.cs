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

    protected override NodeState UpdateStateHelper(BehaviorState state, Ship ship)
    {
        return children[pos].UpdateState(state, ship);
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
        return string.Format("Random\n{0}", pos);
    }
}
