using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanFireWeapon : BehaviorConditional
{
    private Arsenal arsenal;

    private void Awake()
    {
        arsenal = GetComponentInParent<Ship>().GetComponentInChildren<Arsenal>();
    }

    protected override string GetName()
    {
        return "Can fire weapon";
    }

    protected override bool Conditional(BehaviorState state)
    {
        return arsenal.CanFire(state.weaponChoice);
    }
}
