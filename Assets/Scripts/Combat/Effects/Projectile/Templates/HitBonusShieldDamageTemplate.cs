using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBonusShieldDamageTemplate : ProjectileEffectTemplate
{
    [SerializeField]
    private int bonusDamage;

    protected override ProjectileEffect CreateEffect(GameObject obj)
    {
        var p = obj.AddComponent<HitBonusShieldDamage>();
        p.Setup(bonusDamage);
        return p;
    }
}
