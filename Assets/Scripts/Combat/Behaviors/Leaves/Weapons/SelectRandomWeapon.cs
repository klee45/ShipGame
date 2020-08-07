using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectRandomWeapon : BehaviorLeaf
{
    private int last;

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        //Debug.Log("Touch selectweapon");
        state.weaponChoice = Random.Range(0, state.ship.GetArsenal().GetWeapons().Length);
        last = state.weaponChoice;
        return NodeState.Success;
    }

    protected override string GetName()
    {
        return string.Format("Select weapon {0}", last);
    }
}
