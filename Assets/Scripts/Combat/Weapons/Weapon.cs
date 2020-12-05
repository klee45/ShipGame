using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : AWeapon
{
    [SerializeField]
    protected ProjectileTemplate[] projectileTemplates;

    public override void SetupShipSizeMods(Size shipSize)
    {
        base.SetupShipSizeMods(shipSize);
        foreach (ProjectileTemplate template in projectileTemplates)
        {
            template.Setup();
        }
    }

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