using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ProjectileEffect;

public class EffectDictProjectile : EffectDict
{
    public SortedEffectDict<IOnHitEffect> onHits;
    public SortedEffectDict<IOnHitStayEffect> onStays;

    protected override void Awake()
    {
        base.Awake();
        onHits = Link(new SortedEffectDict<IOnHitEffect>());
        onStays = Link(new SortedEffectDict<IOnHitStayEffect>());
    }
}
