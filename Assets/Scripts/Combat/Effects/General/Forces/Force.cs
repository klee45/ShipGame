using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force : AForce, 
    EntityEffect.ITickEffect
{
    [SerializeField]
    private float maxDuration;
    private float duration = 0;
    private bool doesFade;

    public void Setup(Vector2 force, float maxDuration, bool isRelative, bool doesFade)
    {
        this.force = force;
        this.maxDuration = maxDuration;
        this.isRelative = isRelative;
        this.doesFade = doesFade;
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
        dict.movementEffects.Add(this, () => new EffectDict.MovementEffectCase<Force>(true, new EffectDict.EffectList<IMovementEffect, Force>()));
        dict.tickEffects.Add(this, () => new EffectDict.TickEffectCase<Force>(false, new EffectDict.EffectList<ITickEffect, Force>()));
    }

    public override string GetName()
    {
        return "Force";
    }

    protected override Vector3 RelativeHelper(float timeDelta)
    {
        if (doesFade)
        {
            return base.RelativeHelper(timeDelta) * GetFadeMod();
        }
        else
        {
            return base.RelativeHelper(timeDelta);
        }
    }

    protected override Vector3 NotRelativeHelper(float timeDelta)
    {
        if (doesFade)
        {
            return base.NotRelativeHelper(timeDelta) * GetFadeMod();
        }
        else
        {
            return base.NotRelativeHelper(timeDelta);
        }
    }

    private float GetFadeMod()
    {
        return 1 - (duration / maxDuration);
    }
}