﻿using System.Collections;
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
        onHits = new SortedEffectDict<IOnHitEffect>(this);
        onStays = new SortedEffectDict<IOnHitStayEffect>(this);
    }
}
