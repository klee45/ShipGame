using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIsReady : BehaviorConditional
{
    protected override string GetName()
    {
        return "Can fire weapon";
    }

    protected override bool Conditional(BehaviorState state)
    {
        return state.ship.GetArsenal().WeaponIsReady(state.weaponInfo.weaponIndex);
    }
}
