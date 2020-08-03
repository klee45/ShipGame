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

    public override void AddTo(EffectDictProjectile dict)
    {
        dict.onHits.Add(this, () => new EffectDictProjectile.OnHitEffectCase<HitOncePer>(true, new EffectDict.EffectList<IOnHitEffect, HitOncePer>()));
    }

    public void OnHit(Collider2D collision, Collider2D collidee)
    {
        Physics2D.IgnoreCollision(collidee, collision);
        DoDamage(collision, damage);
    }

    public override string GetName()
    {
        return "Hit once per enemy";
    }

    public static readonly EffectTag[] tags = new EffectTag[] { EffectTag.DAMAGE };
    public override EffectTag[] GetTags()
    {
        return tags;
    }
}
