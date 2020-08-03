using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceEndless : AForce
{
    public void Setup(Vector2 force, bool isRelative)
    {
        this.force = force;
        this.isRelative = isRelative;
    }

    public override void AddTo(EffectDict dict)
    {
        dict.movementEffects.Add(this, () => new EffectDict.MovementEffectCase<ForceEndless>(new EffectDict.EffectList<IMovementEffect, ForceEndless>()));
    }

    public override string GetName()
    {
        return "Force";
    }
}