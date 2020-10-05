using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasEnergyToShoot : BehaviorConditional
{
    protected override bool Conditional(BehaviorState state)
    {
        return state.ship.HasEnergyForWeapon(state.weaponInfo.weaponIndex);
    }

    protected override string GetName()
    {
        return "Has energy left to shoot";
    }
}
