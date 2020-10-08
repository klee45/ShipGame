using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeat : BehaviorLink
{
    [SerializeField]
    private NodeState repeatState = NodeState.Running;

    protected override string GetName()
    {
        return "Repeating behavior";
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        NodeState result = child.UpdateState(state);
        //Debug.Log(child + " | " + result + " : " + Mathf.FloorToInt(Time.realtimeSinceStartup));
        switch (result)
        {
            case NodeState.Success:
            case NodeState.Failure:
                child.ResetNode();
                break;
        }
        return repeatState;
    }
}
