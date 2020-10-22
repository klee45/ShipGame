using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitOncePer : ProjectileEffect, ProjectileEffect.IOnHitEffect
{
    [SerializeField]
    private int damage;
    private Ship source;

    public void Setup(int damage, Ship source)
    {
        this.damage = damage;
        this.source = source;
    }

    public override void AddTo(EffectDictProjectile dict)
    {
        dict.onHits.Add(this, () => new EffectDictProjectile.OnHitEffectCase<HitOncePer>(true, new EffectDict.EffectList<IOnHitEffect, HitOncePer>()));
    }

    public void OnHit(Collider2D collision, Collider2D collidee)
    {
        Physics2D.IgnoreCollision(collidee, collision);
        DoDamage(collision, damage, source);
    }

    public override string GetName()
    {
        return "Hit once per enemy";
    }

    public static readonly EffectTag[] tags = new EffectTag[] { EffectTag.Damage };
    public override EffectTag[] GetTags()
    {
        return tags;
    }
}
