using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceEndless : AForce, EffectDict.IEffectAdds
{
    public override void AddTo(EffectDict dict)
    {
        dict.movementEffects.Add(this);
    }
}