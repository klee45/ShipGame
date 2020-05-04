using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedLifespan : GeneralEffect, GeneralEffect.IGeneralEffect
{
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

    protected override void AddToHelper(EffectDict dict)
    {
        dict.generalEffects.Add(this);
    }

    protected override void RemoveFromHelper(EffectDict dict)
    {
        dict.generalEffects.Remove(this);
    }
}
