using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBonusHullDamageTemplate : ProjectileEffectTemplate
{
    [SerializeField]
    private int bonusDamage;
    [SerializeField]
    private bool isHit = false;
    [SerializeField]
    private bool ignoreOther = false;

    protected override ProjectileEffect CreateEffect(GameObject obj)
    {
        var p = obj.AddComponent<HitBonusHullDamage>();
        p.Setup(bonusDamage, isHit, ignoreOther);
        return p;
    }
}
