﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force : AForce , EffectDict.IEffectAdds, GeneralEffect.ITickEffect
{
    [SerializeField]
    private float maxDuration;
    private float duration = 0;
    
    public void Setup(Vector2 force, float maxDuration, bool isRelative)
    {
        this.force = force;
        this.maxDuration = maxDuration;
        this.isRelative = isRelative;
    }

    public void Tick()
    {
        duration += Time.deltaTime;
        if (duration >= maxDuration)
        {
            Destroy(this);
        }
    }

    public override void AddTo(EffectDict dict)
    {
        dict.movementEffects.Add(this);
        dict.tickEffects.Add(this);
    }
}