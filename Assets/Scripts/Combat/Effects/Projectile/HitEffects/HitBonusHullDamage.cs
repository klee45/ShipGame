using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBonusHullDamage : ProjectileEffect, ProjectileEffect.IOnHitEffect, EffectDict.IEffectAdds<ProjectileEffect.IOnHitEffect>
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
        collision.GetComponent<Ship>().GetCombatStats().BonusHullDamage(bonusDamage, isHit, ignoreOther);
    }
}
