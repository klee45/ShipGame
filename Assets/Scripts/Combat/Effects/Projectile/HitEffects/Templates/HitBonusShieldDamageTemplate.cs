using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBonusShieldDamageTemplate : ProjectileEffectTemplate
{
    [SerializeField]
    private int bonusDamage;
    [SerializeField]
    private bool isHit = false;

    protected override ProjectileEffect CreateEffect(GameObject obj)
    {
        var p = obj.AddComponent<HitBonusShieldDamage>();
        p.Setup(bonusDamage, isHit);
        return p;
    }
}
