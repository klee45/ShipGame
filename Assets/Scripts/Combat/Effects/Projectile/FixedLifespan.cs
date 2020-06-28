using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedLifespan : ProjectileEffect, EntityEffect.ITickEffect, EffectDict.IEffectUpdates
{
    protected Timer timer;

    public virtual void Setup(float duration, float start=0)
    {
        timer = gameObject.AddComponent<Timer>();
        timer.Initialize(duration, start);
        timer.OnComplete += () => DestroySelf();
    }

    public override void AddTo(EffectDictProjectile dict)
    {
        dict.tickEffects.AddUpdate(this);
    }

    public virtual IEffect UpdateEffect(IEffect effect, out bool didReplace)
    {
        if (effect is FixedLifespan f)
        {
            f.Setup(timer.GetMaxTime(), timer.GetTime());
        }
        didReplace = false;
        return effect;
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
