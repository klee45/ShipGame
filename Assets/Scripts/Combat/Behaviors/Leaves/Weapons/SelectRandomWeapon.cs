using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectRandomWeapon : BehaviorLeaf
{
    private int last;

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        //Debug.Log("Touch selectweapon");
        state.weaponInfo.weaponIndex = Random.Range(0, state.ship.GetArsenal().GetWeapons().Length);
        last = state.weaponInfo.weaponIndex;
        return NodeState.Success;
    }

    protected override string GetName()
    {
        return string.Format("Select weapon {0}", last);
    }
}
