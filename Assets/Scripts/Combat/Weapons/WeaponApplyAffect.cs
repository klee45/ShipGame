using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponApplyAffect : AWeapon
{
    [SerializeField]
    private ShipEffectTemplate[] templates;

    protected override void FireHelper(Ship owner)
    {
        foreach (ShipEffectTemplate template in templates)
        {
            EffectDictShip e = GetComponentInParent<Ship>().GetEffectsDict();
            ShipEffect effect = template.Create(e.gameObject);
            effect.AddTo(e);
        }
    }

    protected override void InitializeRangeEstimator()
    {
        rangeEstimator.ForceRange(0);
    }
}
