using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanFireWeapon : BehaviorConditional
{
    [SerializeField]
    private Arsenal arsenal = null;

    protected override string GetName()
    {
        return "Can fire weapon";
    }

    protected override bool Conditional(BehaviorState state)
    {
        return arsenal.CanFire(state.weaponChoice);
    }
}
