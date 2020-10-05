using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatState : BehaviorLeaf
{
    [SerializeField]
    private NodeState state;

    protected override string GetName()
    {
        return string.Format("Repeat {0}", state);
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        return this.state;
    }
}
