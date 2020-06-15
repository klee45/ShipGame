using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitOncePer : ProjectileEffect, ProjectileEffect.IOnHitEffect, EffectDict.IEffectAdds
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
        Entity hitEntity = collision.GetComponent<Entity>();
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), hitEntity.GetComponent<Collider2D>());
        DoDamage(collision, damage);
    }
}
