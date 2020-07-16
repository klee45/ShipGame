using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceEndless : AForce, EffectDict.IEffectAdds<EntityEffect.IMovementEffect>
{
    public void Setup(Vector2 force, bool isRelative)
    {
        this.force = force;
        this.isRelative = isRelative;
    }

    public override void AddTo(EffectDict dict)
    {
        dict.movementEffects.Add(this);
    }

    public override string GetName()
    {
        return "Force";
    }
}