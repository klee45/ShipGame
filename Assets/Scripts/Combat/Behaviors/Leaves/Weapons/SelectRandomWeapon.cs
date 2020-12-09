using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectRandomWeapon : BehaviorLeaf
{
    private int last;

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        //Debug.Log("Touch selectweapon");
        Arsenal arsenal = state.ship.GetArsenal();
        //List<AWeapon> weapons = arsenal.GetAllWeapons();
        List<int> slots = arsenal.GetAllWeaponsPairedSlots();
        state.weaponInfo.weaponIndex = slots.GetRandomElement();
        last = state.weaponInfo.weaponIndex;
        return NodeState.Success;
    }

    protected override string GetName()
    {
        return string.Format("Select weapon {0}", last);
    }
}
