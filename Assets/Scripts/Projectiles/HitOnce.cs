using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitOnce : ProjectileMod, ProjectileEffect.IOnHitEffect
{
    public override void AddTo(EffectDictProjectile dict)
    {
        dict.onHits.Add(this);
    }

    public void OnHit(Collider2D collision)
    {
        DoDamage(collision, GetDamage());
        Destroy(gameObject);
    }

    public void Tick() { }
}
