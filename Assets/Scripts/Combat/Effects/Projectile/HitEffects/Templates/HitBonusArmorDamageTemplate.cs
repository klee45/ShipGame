using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBonusArmorDamageTemplate : ProjectileEffectTemplate
{
    [SerializeField]
    private SizeMod bonusDamage;
    [SerializeField]
    private bool isHit = false;

    protected override ProjectileEffect CreateEffect(GameObject obj)
    {
        var p = obj.AddComponent<HitBonusArmorDamage>();
        p.Setup(bonusDamage.ToInt(), isHit);
        return p;
    }
}
