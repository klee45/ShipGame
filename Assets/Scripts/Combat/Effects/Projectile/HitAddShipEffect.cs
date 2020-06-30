using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAddShipEffect : ProjectileEffect, ProjectileEffect.IOnHitEffect, EffectDict.IEffectAdds
{
    [SerializeField]
    ShipEffectTemplate template;

    public void Setup(ShipEffectTemplate template)
    {
        this.template = template;
    }

    public override void AddTo(EffectDictProjectile dict)
    {
        dict.onHits.Add(this);
    }

    public override string GetName()
    {
        return "Hit add ship effect " + template.name;
    }

    public override Tag[] GetTags()
    {
        return TagHelper.empty;
    }

    public void OnHit(Collider2D collision, Collider2D collidee)
    {
        Ship ship = collision.GetComponent<Ship>();
        ShipEffect effect = template.Create(ship.GetEffectsDict().gameObject);
        effect.AddTo(ship.GetEffectsDict());
    }
}
