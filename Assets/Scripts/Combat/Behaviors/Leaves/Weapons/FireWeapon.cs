using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWeapon : BehaviorLeaf
{
    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        //Debug.Log("Touch fireweapon");
        state.weaponInfo.fireWeapon = true;
        return NodeState.Success;
    }

    protected override string GetName()
    {
        return "Fire weapon";
    }
}
