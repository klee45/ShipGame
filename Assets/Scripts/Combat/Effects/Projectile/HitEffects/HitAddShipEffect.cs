using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAddShipEffect : HitAddEffectHelper<ShipEffectTemplate, ShipEffect>
{
    public override void AddTo(EffectDictProjectile dict)
    {
        dict.onHits.Add(this, () => new EffectDictProjectile.OnHitEffectCase<HitAddShipEffect>(true, new EffectDict.EffectList<IOnHitEffect, HitAddShipEffect>()));
    }

    public override string GetName()
    {
        return "Hit add ship effect " + template.name;
    }

    public override void OnHit(Collider2D collision, Collider2D collidee)
    {
        Physics2D.IgnoreCollision(collidee, collision);
        EffectDictShip e = collision.GetComponent<Ship>().GetEffectsDict();
        ShipEffect effect = template.Create(e.gameObject);
        effect.AddTo(e);
    }
}
