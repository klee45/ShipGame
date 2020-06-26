using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitOnce : ProjectileEffect, ProjectileEffect.IOnHitEffect, EffectDict.IEffectUpdates
{
    [SerializeField]
    private int damage;

    public void Setup(int damage)
    {
        this.damage = damage;
    }

    public void OnHit(Collider2D collision, Collider2D collidee)
    {
        DoDamage(collision, damage);
        DestroySelf();
    }

    public override void AddTo(EffectDictProjectile dict)
    {
        dict.onHits.AddUpdate(this);
    }

    public IEffect UpdateEffect(IEffect effect, out bool didReplace)
    {
        if (effect is HitOnce e)
        {
            e.damage += this.damage;
        }
        didReplace = false;
        return effect;
    }

    public override string GetName()
    {
        return "Hit once";
    }

    public static readonly Tag[] tags = new Tag[] { Tag.DAMAGE };
    public override Tag[] GetTags()
    {
        return tags;
    }
}
