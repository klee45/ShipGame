using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedLifespan : ProjectileEffect, ProjectileEffect.IGeneralProjectileEffet, EffectDict.IEffectAdds
{
    private static string FIXED_NAME = "__Fixed Lifespan";

    [SerializeField]
    private float duration;

    public void Setup(float duration)
    {
        this.duration = duration;
    }

    public override void AddTo(EffectDictProjectile dict)
    {
        dict.generalProjectileEffects.Add(this);
    }

    public void Apply(Projectile e)
    {
        Debug.Log("Apply fixed lifespan");
        e.SetDuration(duration);
    }

    public void Cleanup(Projectile e)
    {
    }

    public override string GetName()
    {
        return string.Format("Fixed lifespan {0:0.##}", duration);
    }
    
    public override Tag[] GetTags()
    {
        return TagHelper.empty;
    }
}
