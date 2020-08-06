using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAddBarrierTemplate : ProjectileEffectTemplate
{
    [SerializeField]
    private int val = 50;
    [SerializeField]
    private int limit = 100;

    protected override ProjectileEffect CreateEffect(GameObject obj)
    {
        var effect = obj.AddComponent<HitAddBarrier>();
        effect.Setup(val, limit);
        HitAddBarrier.Helper(GetComponentInParent<Ship>(), val, limit);
        return effect;
    }
}