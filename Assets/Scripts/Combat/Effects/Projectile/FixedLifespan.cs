﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedLifespan : ProjectileEffect, EntityEffect.ITickEffect
{
    [SerializeField]
    private float duration;
    protected Timer timer;

    public virtual void Setup(float duration, float start=0)
    {
        this.duration = duration;
        timer = gameObject.AddComponent<Timer>();
        timer.Initialize(duration, start);
        timer.OnComplete += () => DestroySelf();
    }

    public override void AddTo(EffectDictProjectile dict)
    {
        dict.tickEffects.Add(this, () => new FixedLifespanEffectCase(EffectDict.EffectCaseType.SingleReplace));
    }

    private class FixedLifespanEffectCase : EffectDict.TickEffectCase<FixedLifespan>
    {
        public FixedLifespanEffectCase(EffectDict.EffectCaseType type) : base(type)
        {
        }
    }

    public virtual IEffect UpdateEffect(IEffect effect, out bool didReplace)
    {
        if (effect is FixedLifespan f)
        {
            float time = 0;
            f.Setup(duration, time);
        }
        didReplace = false;
        return effect;
    }

    public EntityEffect.ITickEffect UpdateEffect(EntityEffect.ITickEffect effect, out bool didReplace)
    {
        throw new System.NotImplementedException();
    }

    public virtual void Tick(float timeScale)
    {
        timer.Tick(TimeController.DeltaTime(timeScale));
    }

    public override string GetName()
    {
        return string.Format("Fixed lifespan {0:0.#}/{1:0.#}", timer.GetTime(), timer.GetMaxTime());
    }
    
    public override Tag[] GetTags()
    {
        return TagHelper.empty;
    }
}
