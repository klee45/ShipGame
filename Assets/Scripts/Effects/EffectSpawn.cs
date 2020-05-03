using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectTemplat2e : Template<Effect, GameObject>
{
    public abstract float GetRangeMod(float duration);
}
