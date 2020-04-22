using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWeapon : BehaviorLeaf
{
    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        //Debug.Log("Touch fireweapon");
        state.fireWeapon = true;
        return NodeState.SUCCESS;
    }

    protected override string GetName()
    {
        return "Fire weapon";
    }
}
