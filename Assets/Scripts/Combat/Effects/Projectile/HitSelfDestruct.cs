using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSelfDestruct : ProjectileEffect, ProjectileEffect.IOnHitEffect, EffectDict.IEffectUpdates
{
    public void OnHit(Collider2D collision)
    {
        DestroySelf();
    }

    public override void AddTo(EffectDictProjectile dict)
    {
        dict.onHits.AddUpdate(this);
    }

    public IEffect UpdateEffect(IEffect effect, out bool didReplace)
    {
        didReplace = false;
        return effect;
    }

    public override string GetName()
    {
        return "Self destruct on hit";
    }
    
    public override Tag[] GetTags()
    {
        return TagHelper.empty;
    }
}
