using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBonusShieldDamage : ProjectileEffect, ProjectileEffect.IOnHitEffect, EffectDict.IEffectAdds
{
    [SerializeField]
    private int bonusDamage;

    public void Setup(int bonusDamage)
    {
        this.bonusDamage = bonusDamage;
    }

    public override void AddTo(EffectDictProjectile dict)
    {
        dict.onHits.Add(this);
    }

    public override string GetName()
    {
        return string.Format("Bonus shield damage ({0}) effect", bonusDamage);
    }

    private static Tag[] tags = new Tag[] { Tag.DAMAGE, Tag.SHIELD_DAMAGE };
    public override Tag[] GetTags()
    {
        return tags;
    }

    public void OnHit(Collider2D collision, Collider2D collidee)
    {
        collision.GetComponent<Ship>().GetCombatStats().TakeShieldDamage(bonusDamage);
    }
}
