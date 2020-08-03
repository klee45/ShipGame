using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSelfDestruct : ProjectileEffect, ProjectileEffect.IOnHitEffect
{
    public void OnHit(Collider2D collision, Collider2D collidee)
    {
        DestroySelf();
    }

    public override void AddTo(EffectDictProjectile dict)
    {
        dict.onHits.Add(this, () => new HitSelfDestructEffectCase(true, new EffectDict.EffectSingleKeep<IOnHitEffect, HitSelfDestruct>()));
    }

    private class HitSelfDestructEffectCase : EffectDictProjectile.OnHitEffectCase<HitSelfDestruct>
    {
        public HitSelfDestructEffectCase(bool isVisible, EffectDict.IEffectList<IOnHitEffect, HitSelfDestruct> effectsList) : base(isVisible, effectsList)
        {
        }

        public override int GetPriority()
        {
            return Constants.Effects.LATE_PRIORITY;
        }
    }

    public IOnHitEffect UpdateEffect(IOnHitEffect effect, out bool didReplace)
    {
        didReplace = false;
        return effect;
    }

    public override string GetName()
    {
        return "Self destruct on hit";
    }
    
    public override EffectTag[] GetTags()
    {
        return TagHelper.empty;
    }
}
