using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSelfDestructTemplate : ProjectileEffectTemplate
{
    private void Awake()
    {
        priority = -1000;
    }

    protected override ProjectileEffect CreateEffect(GameObject obj)
    {
        HitSelfDestruct hitSelfDestruct = obj.AddComponent<HitSelfDestruct>();
        return hitSelfDestruct;
    }
}

public class HitSelfDestruct : ProjectileEffect, ProjectileEffect.IOnHitEffect
{
    public void OnHit(Collider2D collision)
    {
        DestroySelf();
    }

    public override void AddTo(EffectDictProjectile dict)
    {
        dict.onHits.Add(this);
    }
}
