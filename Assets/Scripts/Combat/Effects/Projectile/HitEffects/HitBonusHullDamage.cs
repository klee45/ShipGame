using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBonusHullDamage : ProjectileEffect, ProjectileEffect.IOnHitEffect
{
    [SerializeField]
    private int bonusDamage;
    [SerializeField]
    private bool isHit = false;
    [SerializeField]
    private bool ignoreOther = false;

    public void Setup(int bonusDamage, bool isHit, bool ignoreOther)
    {
        this.bonusDamage = bonusDamage;
        this.isHit = isHit;
        this.ignoreOther = ignoreOther;
    }

    public override void AddTo(EffectDictProjectile dict)
    {
        dict.onHits.Add(this, () => new EffectDictProjectile.OnHitEffectCase<HitBonusHullDamage>(new EffectDict.EffectList<IOnHitEffect, HitBonusHullDamage>()));
    }

    public override string GetName()
    {
        return string.Format("Bonus shield damage ({0}) effect", bonusDamage);
    }

    private static EffectTag[] tags = new EffectTag[] { EffectTag.DAMAGE, EffectTag.SHIELD_DAMAGE };
    public override EffectTag[] GetTags()
    {
        return tags;
    }

    public void OnHit(Collider2D collision, Collider2D collidee)
    {
        collision.GetComponent<Ship>().GetCombatStats().BonusHullDamage(bonusDamage, isHit, ignoreOther);
    }
}
