using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWeapon : BehaviorLeaf
{
    private static string text = "Fire weapon";

    protected override NodeState UpdateStateHelper(BehaviorState state, Ship ship)
    {
        //Debug.Log("Touch fireweapon");
        state.fireWeapon = true;
        return NodeState.SUCCESS;
    }

    protected override string GetName()
    {
        return text;
    }
}
