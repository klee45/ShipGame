using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelfDestruct : AWeapon
{
    protected override void FireHelper(Ship owner)
    {
        owner.GetCombatStats().TakeDamage(9999999, owner);
    }

    protected override void InitializeRangeEstimator()
    {
        rangeEstimator.ForceRange(0);
    }
}
