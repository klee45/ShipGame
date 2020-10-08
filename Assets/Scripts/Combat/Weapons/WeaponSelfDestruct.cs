using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelfDestruct : AWeapon
{
    protected override void FireHelper()
    {
        GetComponentInParent<Ship>().GetCombatStats().TakeDamage(9999999);
    }

    protected override void InitializeRangeEstimator()
    {
        rangeEstimator.ForceRange(0);
    }

    protected override void SetProjectileTemplateTeams(Team team) { }
}
