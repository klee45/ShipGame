using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeat : BehaviorLeaf
{
    [SerializeField]
    private NodeState repeatState;

    protected override string GetName()
    {
        return string.Format("Repeat {0}", repeatState);
    }

    protected override NodeState UpdateStateHelper(BehaviorState state, Ship ship)
    {
        return repeatState;
    }
}
