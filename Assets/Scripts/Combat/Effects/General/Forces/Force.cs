using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force : AForce, 
    EntityEffect.ITickEffect
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

    public void Tick(float timeScale)
    {
        duration += TimeController.DeltaTime(timeScale);
        if (duration >= maxDuration)
        {
            Destroy(this);
        }
    }

    public override void AddTo(EffectDict dict)
    {
        dict.movementEffects.Add(this, () => new EffectDict.MovementEffectCase<Force>(new EffectDict.EffectList<IMovementEffect, Force>()));
        dict.tickEffects.Add(this, () => new EffectDict.TickEffectCase<Force>(new EffectDict.EffectList<ITickEffect, Force>()));
    }

    public override string GetName()
    {
        return "Force";
    }
}