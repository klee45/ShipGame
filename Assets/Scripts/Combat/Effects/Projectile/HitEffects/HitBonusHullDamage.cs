using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBonusHullDamage : ProjectileEffect, ProjectileEffect.IOnHitEffect
{
    [SerializeField]
    private int bonusDamage;
    [SerializeField]
    private bool isHit = false;
    
    public void Setup(int bonusDamage, bool isHit)
    {
        this.bonusDamage = bonusDamage;
        this.isHit = isHit;
    }

    public override void AddTo(EffectDictProjectile dict)
    {
        dict.onHits.Add(this, () => new EffectDictProjectile.OnHitEffectCase<HitBonusHullDamage>(true, new EffectDict.EffectList<IOnHitEffect, HitBonusHullDamage>()));
    }

    public override string GetName()
    {
        return string.Format("Bonus hull damage ({0}) effect", bonusDamage);
    }

    private static EffectTag[] tags = new EffectTag[] { EffectTag.Damage, EffectTag.HullDamage };
    public override EffectTag[] GetTags()
    {
        return tags;
    }

    public void OnHit(Collider2D collision, Collider2D collidee)
    {
        GetShip(collision).GetCombatStats().BonusHullDamage(bonusDamage, isHit);
    }
}
