using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBonusHullDamageTemplate : ProjectileEffectTemplate
{
    [SerializeField]
    private SizeModNumber bonusDamage;
    [SerializeField]
    private bool isHit = false;

    protected override ProjectileEffect CreateEffect(GameObject obj)
    {
        var p = obj.AddComponent<HitBonusHullDamage>();
        p.Setup(bonusDamage.ToInt(), isHit);
        return p;
    }
}
