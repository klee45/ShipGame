using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponOneShot : Weapon
{
    protected override void FireHelper()
    {
        Projectile p = CreateProjectile();
        AttachToManager(p);
    }

    protected override void InitializeRangeEstimator()
    {
        rangeEstimator.Estimate(
            projectileTemplate.GetVelocityTemplate(),
            projectileTemplate.GetBoundsY(),
            projectileTemplate.GetLifespan());
    }
}