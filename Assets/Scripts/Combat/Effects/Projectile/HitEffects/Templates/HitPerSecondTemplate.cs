using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitPerSecondTemplate : ProjectileEffectTemplate
{
    [SerializeField]
    private SizeMod damage;
    [SerializeField]
    private float duration;
    [SerializeField]
    private bool destroyOnEnd = true;

    protected override ProjectileEffect CreateEffect(GameObject obj)
    {
        HitPerSecond hps = obj.AddComponent<HitPerSecond>();
        hps.Setup(damage.ToInt(), duration, destroyOnEnd, obj.GetComponent<Projectile>().GetOwner());
        return hps;
    }
}