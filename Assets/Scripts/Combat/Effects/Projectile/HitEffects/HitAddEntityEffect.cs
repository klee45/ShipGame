using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAddEntityEffect : HitAddEffectHelper<EntityEffectTemplate, EntityEffect>
{
    public override void AddTo(EffectDictProjectile dict)
    {
        dict.onHits.Add(this, () => new EffectDictProjectile.OnHitEffectCase<HitAddEntityEffect>(true, new EffectDict.EffectList<IOnHitEffect, HitAddEntityEffect>()));
    }

    public override string GetName()
    {
        return "Hit add entity effect " + template.name;
    }

    public override void OnHit(Collider2D collision, Collider2D collidee)
    {
        EffectDict e = collision.GetComponent<Entity>().GetGeneralEffectDict();
        EntityEffect effect = template.Create(e.gameObject);
        effect.AddTo(e);
    }
}
