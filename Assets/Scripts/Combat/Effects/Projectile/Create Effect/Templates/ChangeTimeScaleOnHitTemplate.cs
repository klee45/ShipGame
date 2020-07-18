using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTimeScaleOnHitTemplate : ProjectileEffectTemplate
{
    [SerializeField]
    private int timeScaleBonus = 0;
    [SerializeField]
    private int limit;

    protected override ProjectileEffect CreateEffect(GameObject obj)
    {
        var p = obj.AddComponent<ChangeTimeScaleOnHit>();
        p.Setup(timeScaleBonus, limit);
        return p;
    }
}
