using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedLifespan : GeneralEffect, GeneralEffect.IGeneralEffect, EffectDict.IEffectUpdates
{
    private static string FIXED_NAME = "__Fixed Lifespan";

    [SerializeField]
    private float duration;

    public void Setup(float duration)
    {
        this.duration = duration;
    }

    public void Apply(Entity e)
    {
        Destroy(e.gameObject, duration);
    }

    public override void AddTo(EffectDict dict)
    {
        dict.generalEffects.AddUpdate(this);
    }

    public IEffect UpdateEffect(IEffect effect, out bool didReplace)
    {
        if (effect is FixedLifespan e)
        {
            e.duration = this.duration;
        }
        didReplace = false;
        return effect;
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
