using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRepeat : AWeapon
{
    [SerializeField]
    private ProjectileTemplate template;
    [SerializeField]
    private float[] delays;

    protected override void FireHelper(Ship owner)
    {
        foreach (float delay in delays)
        {
            StartCoroutine(CreateProjectileCoroutine(template, owner, delay));
        }
    }

    protected override void InitializeRangeEstimator()
    {
        rangeEstimator.Estimate(template);
    }
}
