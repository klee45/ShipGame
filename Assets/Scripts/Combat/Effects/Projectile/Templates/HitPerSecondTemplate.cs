using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitPerSecondTemplate : ProjectileEffectTemplate
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private float duration;
    [SerializeField]
    private bool destroyOnEnd = true;

    protected override ProjectileEffect CreateEffect(GameObject obj)
    {
        HitPerSecond hps = obj.AddComponent<HitPerSecond>();
        hps.Setup(damage, duration, destroyOnEnd);
        return hps;
    }
}