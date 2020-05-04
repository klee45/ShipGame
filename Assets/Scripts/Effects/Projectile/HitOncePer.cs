using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitOncePer : ProjectileEffect, ProjectileEffect.IOnHitEffect
{
    [SerializeField]
    private int damage;

    public void Setup(int damage)
    {
        this.damage = damage;
    }

    protected override void AddToHelper(EffectDictProjectile dict)
    {
        dict.onHits.Add(this);
    }

    protected override void RemoveFromHelper(EffectDictProjectile dict)
    {
        dict.onHits.Remove(this);
    }

    public void OnHit(Collider2D collision)
    {
        Entity hitEntity = collision.GetComponent<Entity>();
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), hitEntity.GetComponent<Collider2D>());
        DoDamage(collision, damage);
    }
}
