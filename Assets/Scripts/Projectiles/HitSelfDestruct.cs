using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSelfDestructTemplate : EffectTemplate
{
    private void Awake()
    {
        priority = -1000;
    }

    public override float GetRangeMod(float duration)
    {
        return 0;
    }

    protected override Effect CreateEffect(GameObject obj)
    {
        HitSelfDestruct hitSelfDestruct = obj.AddComponent<HitSelfDestruct>();
        return hitSelfDestruct;
    }
}

public class HitSelfDestruct : ProjectileMod, ProjectileEffect.IOnHitEffect
{
    public void OnHit(Collider2D collision)
    {
        DestroySelf();
    }

    public void Tick() { }

    public override void AddTo(EffectDictProjectile dict)
    {
        dict.onHits.Add(this);
    }
}
