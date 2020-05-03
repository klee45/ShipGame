using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ProjectileEffect;

public class EffectDictProjectile : EffectDict
{
    public EffectContainer<IOnHitEffect> onHits;
    public EffectContainer<IOnHitStayEffect> onStays;

    public void Awake()
    {
        onHits = new EffectContainer<IOnHitEffect>();
        onStays = new EffectContainer<IOnHitStayEffect>();
    }

    public override void SortAll()
    {
        onHits.Sort();
        onStays.Sort();
    }

    public override void Tick()
    {
        base.Tick();
        onHits.Tick();
        onStays.Tick();
    }
}
