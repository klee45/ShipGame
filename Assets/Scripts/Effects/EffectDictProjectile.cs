using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ProjectileEffect;

public class EffectDictProjectile : EffectDict
{
    public EffectContainer<IOnHitEffect> onHits;
    public EffectContainer<IOnHitStayEffect> onStays;

    protected override void Awake()
    {
        base.Awake();
        onHits = new EffectContainer<IOnHitEffect>();
        onStays = new EffectContainer<IOnHitStayEffect>();
    }

    public override void SortAll()
    {
        base.SortAll();
        onHits.Sort();
        onStays.Sort();
    }
}
