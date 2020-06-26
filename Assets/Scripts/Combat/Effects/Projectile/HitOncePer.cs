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

    public void OnHit(Collider2D collision, Collider2D collidee)
    {
        Physics2D.IgnoreCollision(collidee, collision);
        DoDamage(collision, damage);
    }

    public override string GetName()
    {
        return "Hit once per enemy";
    }

    public static readonly Tag[] tags = new Tag[] { Tag.DAMAGE };
    public override Tag[] GetTags()
    {
        return tags;
    }
}
