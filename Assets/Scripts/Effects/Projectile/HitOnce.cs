using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitOnce : ProjectileEffect, ProjectileEffect.IOnHitEffect
{
    [SerializeField]
    private int damage;

    public void Setup(int damage)
    {
        this.damage = damage;
    }

    public override void AddTo(EffectDictProjectile dict)
    {
        dict.onHits.Add(this);
    }

    public void OnHit(Collider2D collision)
    {
        DoDamage(collision, damage);
        DestroySelf();
    }
}
