using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSelfDestruct : ProjectileEffect, ProjectileEffect.IOnHitEffect
{
    public void OnHit(Collider2D collision)
    {
        DestroySelf();
    }

    protected override void AddToHelper(EffectDictProjectile dict)
    {
        dict.onHits.Add(this);
    }

    protected override void RemoveFromHelper(EffectDictProjectile dict)
    {
        dict.onHits.Remove(this);
    }
}
