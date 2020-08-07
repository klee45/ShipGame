using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectWeapon : BehaviorLeaf
{
    [SerializeField]
    private int choice;

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        //Debug.Log("Touch selectweapon");
        state.weaponChoice = choice;
        return NodeState.Success;
    }

    protected override string GetName()
    {
        return string.Format("Select weapon {0}", choice);
    }
}
