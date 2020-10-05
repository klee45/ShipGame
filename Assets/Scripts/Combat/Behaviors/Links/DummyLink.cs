using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyLink : BehaviorLink
{
    [SerializeField]
    private string nodeName;

    protected override string GetName()
    {
        return nodeName;
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        NodeState result = child.UpdateState(state);
        Debug.Log(nodeName + " updated " + result);
        return result;
    }
}
