using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAddBarrierTemplate : ProjectileEffectTemplate
{
    [SerializeField]
    private SizeModNumber val;
    [SerializeField]
    private SizeModNumber limit;

    protected override ProjectileEffect CreateEffect(GameObject obj)
    {
        var effect = obj.AddComponent<HitAddBarrier>();
        int val = this.val.ToInt();
        int limit = this.limit.ToInt();
        effect.Setup(val, limit);
        HitAddBarrier.Helper(GetComponentInParent<Ship>(), val, limit);
        return effect;
    }
}