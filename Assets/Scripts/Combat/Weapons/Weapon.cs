using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : AWeapon
{
    [SerializeField]
    protected ProjectileTemplate[] projectileTemplates;

    protected override void FireHelper(Ship owner)
    {
        foreach (ProjectileTemplate template in projectileTemplates)
        {
            StartCoroutine(CreateProjectileCoroutine(template, owner, template.GetDelay()));
        }
    }

    protected override void InitializeRangeEstimator()
    {
        rangeEstimator.Estimate(projectileTemplates);
    }
}