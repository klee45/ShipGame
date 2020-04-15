using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanFireWeapon : BehaviorConditional
{
    [SerializeField]
    private Arsenal arsenal = null;
    [SerializeField]
    private NodeState failState = NodeState.RUNNING;

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        if (arsenal.GetWeapons()[state.weaponChoice].IsReady())
        {
            return child.UpdateState(state);
        }
        else
        {
            return failState;
        }
    }

    protected override string GetName()
    {
        return "Can fire weapon";
    }
}
