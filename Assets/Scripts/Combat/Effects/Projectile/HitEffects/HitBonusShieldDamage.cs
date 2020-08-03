using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBonusShieldDamage : ProjectileEffect, ProjectileEffect.IOnHitEffect
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
        dict.onHits.Add(this, () => new EffectDictProjectile.OnHitEffectCase<HitBonusShieldDamage>(true, new EffectDict.EffectList<IOnHitEffect, HitBonusShieldDamage>()));
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
        collision.GetComponent<Ship>().GetCombatStats().BonusShieldDamage(bonusDamage, isHit);
    }
}
